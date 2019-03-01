using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class HookLauncher : MonoBehaviour
{
    public int playerId;
    private Player player;

    public const float LaunchCooldown = .25f;
    public float LaunchVelocity = 150f;
    private static Object HookPrefab;
    float lastShotTime = -1;
    private bool IsAiming = false;
    private GameObject prior_hook;

    private LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public float width = .1f;
    public int lengthOfLineRenderer = 2;

    public GameObject game_player;
    private bool can_fire = true;
    private int layer_mask;

    public GameObject hook;

    public bool in_start_menu = false;
    public Canvas canvas;
    bool done = false;
    bool done2 = false;

    public GameObject scene_manager;
    private Scene_Manager scene;

    public float travel_speed = 150f;

    public bool canShootOwnPlatform;

    // Start is called before the first frame update
    void Start()
    {
        layer_mask = (1 << 8 | 1 << 9);
        layer_mask = ~layer_mask;
        HookPrefab = Resources.Load("Prefabs/Hook");

        if (scene_manager != null)
        {
            scene = scene_manager.GetComponent<Scene_Manager>();
            canShootOwnPlatform = scene.canShootOwnPlatform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void Awake()
    {
        playerId = GetComponentInParent<Movement>().playerId;
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        ProcessShooting();

        if (prior_hook)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.sortingLayerName = "Lines";
            lineRenderer.SetWidth(width, width);
            lineRenderer.SetColors(c1, c2);
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, prior_hook.transform.position);
        }
    }
    void UpdateAim()
    {
        if (in_start_menu)
        {
            Start_Menu start = canvas.GetComponent<Start_Menu>();
            float AimAngle = 0;
            if (start.select == Start_Menu.selection.start)
            {
                AimAngle = CalculateAimAngle(0f, -0.9f);
            }

            if (start.select == Start_Menu.selection.tutorial)
            {
                AimAngle = CalculateAimAngle(0f, 0.9f);
            }

            if (start.select == Start_Menu.selection.credits)
            {
                AimAngle = CalculateAimAngle(-0.5f, -0.9f);
            }

            if (start.select == Start_Menu.selection.quit)
            {
                AimAngle = CalculateAimAngle(-0.5f, 0.9f);
            }

            DisplayAimReticle(AimAngle);
        }

        else
        {
            float Horizontal = player.GetAxis("Right Horizontal");
            float Vertical = player.GetAxis("Right Vertical");
            IsAiming = GameInput.SignificantStickInput(Vertical, Horizontal);
            if (IsAiming)
            {
                Movement outer_collision = GetComponentInParent<Movement>();
                GrapplingHook grapple_collision = hook.GetComponent<GrapplingHook>();
                float AimAngle = CalculateAimAngle(Vertical, Horizontal);
                float AimAngle2 = AimAngle * 0.0174533f;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(AimAngle2), Mathf.Sin(AimAngle2)),
                    1000, layer_mask);

                can_fire = true;

                DisplayAimReticle(AimAngle);
            }
            else
            {
                HideAimReticle();
            }
        }
    }
    void ProcessShooting()
    {
        if (in_start_menu)
        {
            Start_Menu start = canvas.GetComponent<Start_Menu>();
            if (start.start_clicked)
            {
                AttemptFire();
            }
        }

        else
        {

            bool Swing = player.GetButtonDown("Fire 2");
            bool Shoot = player.GetButtonDown("Fire Hook");
            if (Shoot)
            {
                AttemptFire();
                Debug.Log("test2");
            }
            else if (Swing)
            {
                Debug.Log("test");
                AttemptSwingFire();

            }
        }
    }
    private float CalculateAimAngle(float Vertical, float Horizontal)
    {
        return Mathf.Atan2(Vertical, Horizontal) * Mathf.Rad2Deg;
    }
    private void HideAimReticle()
    {
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
    private void DisplayAimReticle(float angle)
    {
        float time = Time.time;
        if (time < (lastShotTime + LaunchCooldown) | !can_fire)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void AttemptFire()
    {
        if (scene_manager != null)
        {
            scene = scene_manager.GetComponent<Scene_Manager>();
        }

        if (scene == null || scene.can_move)
        {
            float time = Time.time;
            if (in_start_menu && !done)
            {
                transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                prior_hook = SpawnHook(transform.position + transform.right * 5f,
                transform.rotation,
                transform.right * LaunchVelocity);
                done = true;
            }

            else
            {
                Fire();
            }
        }
    }
    void AttemptSwingFire()
    {
        Fire();
        if(prior_hook)
        {
            prior_hook.GetComponent<GrapplingHook>().swingHook = true;
            prior_hook.GetComponent<Rigidbody2D>().velocity *= 1.5f;
        }


    }
    void Fire()
    {
        float time = Time.time;
        if (time > (lastShotTime + LaunchCooldown) && IsAiming && can_fire)
        {
            if (prior_hook)
            {
                Destroy(prior_hook);
            }
            lastShotTime = time;

            prior_hook = SpawnHook(transform.position + transform.right * 5f,
                transform.rotation,
                transform.right * LaunchVelocity);
        }
    }



    private GameObject SpawnHook(Vector2 position, Quaternion rotation, Vector2 velocity)
    {
        Transform playerTransform = transform.parent;
        GameObject newHook = (GameObject)Instantiate(HookPrefab,
                                            position,
                                            rotation);
        newHook.GetComponent<GrapplingHook>().hook_speed = travel_speed;
        newHook.GetComponent<Rigidbody2D>().velocity = velocity;
        newHook.GetComponent<GrapplingHook>().Shooter = transform.parent.gameObject;
        lineRenderer = newHook.GetComponent<LineRenderer>();
        return newHook;

    }

    public void startCall()
    {
        AttemptFire();
    }

    private void Create(float speed)
    {

    }
}