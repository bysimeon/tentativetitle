using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HookLauncher : MonoBehaviour
{
    public int playerId;
    private Player player;

    public const float LaunchCooldown = 1f;
    public const float LaunchVelocity = 100f;
    private static Object HookPrefab;
    float lastShotTime = -1;
    private bool IsAiming = false;
    private GameObject prior_hook;

    private  LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 2;

    public GameObject game_player;
    private bool can_fire = true;
    private bool is_fired = false;

    public GameObject hook;

    // Start is called before the first frame update
    void Start()
    {
        HookPrefab = Resources.Load("Prefabs/Hook");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void Awake()
    {
        playerId = GetComponentInParent<Outer_Movement>().playerId;
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        ProcessShooting();

        if (prior_hook)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, prior_hook.transform.position);
        }
    }
    void UpdateAim()
    {
        float Horizontal = player.GetAxis("Right Horizontal");
        float Vertical = player.GetAxis("Right Vertical");
        IsAiming = GameInput.SignificantStickInput(Vertical, Horizontal);
        if (IsAiming)
        {
            Outer_Movement outer_collision = GetComponentInParent<Outer_Movement>();
            GrapplingHook grapple_collision = hook.GetComponent<GrapplingHook>();
            float AimAngle = CalculateAimAngle(Vertical, Horizontal);
            float AimAngle2 = AimAngle * 0.0174533f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(AimAngle2), Mathf.Sin(AimAngle2)),
                1000);

            if (hit.transform.gameObject == outer_collision.getCollider())
            {
                can_fire = false;
            }

            else if (outer_collision.getLocation().Equals("air") && hit.transform.gameObject == outer_collision.get_grapple_collision())
            {
                can_fire = false;
            }

            else
            {
                can_fire = true;
            }

            DisplayAimReticle(AimAngle);
        }
        else
        {
            HideAimReticle();
        }
    }
    void ProcessShooting()
    {
        bool Shoot = player.GetButtonDown("Fire Hook");
        if (Shoot)
        {
            AttemptFire();
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
        float time = Time.time;

        if (time > (lastShotTime + LaunchCooldown) && IsAiming && can_fire)
        {
            is_fired = true;
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
        newHook.GetComponent<Rigidbody2D>().velocity = velocity;
        newHook.GetComponent<GrapplingHook>().Player = transform.parent.gameObject;
        lineRenderer = newHook.GetComponent<LineRenderer>();
        return newHook;

    }
}
