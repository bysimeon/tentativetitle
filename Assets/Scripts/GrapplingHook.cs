using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GrapplingHook : MonoBehaviour
{
    private bool planted;
    Quaternion rotation;
    Vector3 position;
    Transform plantedtransform;
    public GameObject Shooter;
    public float ray_distance;
    public bool same_surface = false;
    private GameObject platform_collider;
    private Object explodePrefab;

    public bool swingHook;

    public GameObject start_collider;

    public Button start;

    public float hook_speed;

    private void Start()
    {
        explodePrefab = Resources.Load("Prefabs/explodeHookEffect");
    }

    private void Awake()
    {
        explodePrefab = Resources.Load("Prefabs/explodeHookEffect");
    }

    void LateUpdate()
    {
        if(planted)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Shooter != null)
        {
            Movement outer_collision = Shooter.GetComponent<Movement>();
            if ((collision.gameObject.tag == "Outer Platform" ||
                collision.gameObject.tag == "Inner Platform" || 
                collision.gameObject.tag == "Inner Platform Corner") &&
                planted == false)
            {
                Movement ShooterMovement = Shooter.
                    GetComponent<Movement>();
                ShooterMovement.DetachFromPlatform();
                PlantHook();
                if(swingHook)
                {
                    Shooter.GetComponent<Movement>().swinging = true;
                    Vector3 hookPosition = gameObject.transform.position;
                    Shooter.GetComponent<Movement>().rotationPoint = hookPosition;

                    Vector3 shooterPosition = Shooter.gameObject.transform.position;
                    float ropeLength = Vector3.Distance(shooterPosition, hookPosition);

                    Shooter.GetComponent<Movement>().rotationRadius = ropeLength;

                }
                else
                {
                    DragShooterToHook();
                }
                ShooterMovement.find_grapple_collision(collision.gameObject);
            }

            if(collision.gameObject.GetComponent<Movement>() && Shooter != collision.gameObject)
            {
                if (swingHook)
                {
                    Explode();
                }
                else
                {
                    var damageManager = GameObject.Find("Damage_Manager");
                    damageManager.GetComponent<Damage_Player>().
                        DamagePlayer(collision.gameObject, collision, gameObject);
                    Destroy(gameObject);
                }


            }

            if(collision.gameObject.GetComponent<GrapplingHook>())
            {
                Explode();
            }
        }
    }
    void Explode()
    {
        GameObject explosion = (GameObject)Instantiate(explodePrefab,
                                                      transform.position,
                                                      transform.rotation);
        var explosionParticles = explosion.GetComponent<ParticleSystem>().main;
        var playerColor = PlayerColors.getPlayerColor(Shooter);
        explosionParticles.startColor = new Color(playerColor.r/255,
                                                 playerColor.g/255,
                                                 playerColor.b/255);
        explosionParticles.startSpeed = gameObject.GetComponent<Rigidbody2D>().
                                        velocity.magnitude;
        Destroy(gameObject);
    }


    void PlantHook()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeAll;
        planted = true;
        rotation = transform.rotation;
        position = transform.position;
    }
    // TODO: Clean this up
    void DragShooterToHook()
    {
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Shooter.transform.position;
        Vector3 PlayerVelocity = Vector3.Normalize(HookPosition - PlayerPosition) *
                                        hook_speed;
        Shooter.GetComponent<Rigidbody2D>().velocity = PlayerVelocity;

    }

    void FixedUpdate()
    {
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Vector3.zero;

        if (Shooter != null)
        {
            PlayerPosition = Shooter.transform.position;

            if (planted && Shooter.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                //Debug.Log("Making it die");
                Destroy(gameObject);
            }

            if (planted && Vector3.Magnitude(HookPosition - PlayerPosition) < 10f)
            {
                Destroy(gameObject);
            }
        }
    }

    public GameObject getCollider()
    {
        return platform_collider;
    }
}
