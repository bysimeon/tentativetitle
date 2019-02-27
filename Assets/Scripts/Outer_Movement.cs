using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

//Defining the vertical and horizontal movement of the player along outer platforms (NO GRAPPLING)
public class Outer_Movement : MonoBehaviour
{
    public int playerId;
    private Player player;

    public enum location { outer, inner, in_air };
    public location loc;
    public enum rotation { up, down, right, left };
    public rotation rot;
    public Rigidbody2D rb;

    public float speed;
    public float ray_distance_vertical;
    public float ray_distance_horizontal;
    public float ray_distance_in_air_vertical;
    public float ray_distance_in_air_horizontal;
    private float ray_distance_in_air_1;
    private float ray_distance_in_air_2;

    public Vector3 bottom_to_right_shift;
    public Vector3 right_to_bottom_shift;
    public Vector3 bottom_to_left_shift;
    public Vector3 left_to_bottom_shift;
    public Vector3 top_to_right_shift;
    public Vector3 right_to_top_shift;
    public Vector3 top_to_left_shift;
    public Vector3 left_to_top_shift;

    public Vector3 top_to_right;
    public Vector3 right_to_top;
    public Vector3 top_to_left;
    public Vector3 left_to_top;
    public Vector3 bottom_to_left;
    public Vector3 left_to_bottom;
    public Vector3 bottom_to_right;
    public Vector3 right_to_bottom;

    private bool rotated_up = false;
    private bool rotated_right = false;
    private bool rotated_left = false;
    private bool rotated_down = false;

    private GameObject collider = null;
    private GameObject grapple_collision = null;
    private int layer_mask;

    public GameObject scene_manager;
    private Scene_Manager scene;

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        layer_mask = (1 << 8 | 1 << 9);
        layer_mask = ~layer_mask;
        loc = location.in_air;
        rotated_up = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per time interval
    void FixedUpdate()
    {
        RaycastHit2D DownRaycast = new RaycastHit2D();
        RaycastHit2D UpRaycast = new RaycastHit2D();
        RaycastHit2D RightRaycast = new RaycastHit2D();
        RaycastHit2D LeftRaycast = new RaycastHit2D();

        if (rot == rotation.up | rot == rotation.down)
        {
            ray_distance_in_air_1 = ray_distance_in_air_vertical;
            ray_distance_in_air_2 = ray_distance_in_air_horizontal;
        }
        else
        {
            ray_distance_in_air_1 = ray_distance_in_air_horizontal;
            ray_distance_in_air_2 = ray_distance_in_air_vertical;
        }

        DownRaycast = Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_in_air_1, layer_mask);
        UpRaycast = Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_in_air_1, layer_mask);
        RightRaycast = Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_in_air_2, layer_mask);
        LeftRaycast = Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_in_air_2, layer_mask);

        if (scene_manager != null)
        {
            scene = scene_manager.GetComponent<Scene_Manager>();
        }

        if (scene != null)
        {
            if (scene.can_move)
            {
                //Attaching to platform when grappling
                if (loc == location.in_air)
                {
                    if (DownRaycast)
                    {
                        if (DownRaycast.collider.gameObject.tag == "Outer Platform")
                        {
                            rot = rotation.up;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                            ConstrainToHorizontalSurface();
                        }
                        else if (DownRaycast.collider.gameObject.tag == "Inner Platform")
                        {
                            rot = rotation.up;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                            ConstrainToHorizontalSurface();
                        }
                        else if (DownRaycast.collider.gameObject.tag == "Inner Platform Corner")
                        {
                            Debug.Log("Corner Hit by Down Raycast");
                            rot = rotation.up;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                            ConstrainToHorizontalSurface();
                        }
                    }
                    else if (
                      UpRaycast)
                    {
                        if (UpRaycast.collider.gameObject.tag == "Outer Platform")
                        {
                            rot = rotation.down;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                            ConstrainToHorizontalSurface();
                        }
                        else if (UpRaycast.collider.gameObject.tag == "Inner Platform")
                        {
                            Debug.Log('8'); 
                            rot = rotation.down;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                            ConstrainToHorizontalSurface();
                        }
                        else if (UpRaycast.collider.gameObject.tag == "Inner Platform Corner")
                        {
                            Debug.Log("Corner Hit by Up Raycast");
                            rot = rotation.down;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                            ConstrainToHorizontalSurface();
                        }
                    }
                    else if (
                      RightRaycast)
                    {
                        Debug.Log('1');
                        if (RightRaycast.collider.gameObject.tag == "Outer Platform")
                        {
                            rot = rotation.right;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                            ConstrainToVerticalSurface();
                        }
                        else if (RightRaycast.collider.gameObject.tag == "Inner Platform")
                        {
                            Debug.Log('2'); 
                            rot = rotation.right;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                            ConstrainToVerticalSurface();
                        }
                        else if (RightRaycast.collider.gameObject.tag == "Inner Platform Corner")
                        {
                            Debug.Log("Corner Hit by Right Raycast");
                            rot = rotation.right;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                            ConstrainToVerticalSurface();
                        }
                    }
                    else if (
                      LeftRaycast)
                    {
                        if (LeftRaycast.collider.gameObject.tag == "Outer Platform")
                        {
                            rot = rotation.left;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                            ConstrainToVerticalSurface();
                        }
                        else if (LeftRaycast.collider.gameObject.tag == "Inner Platform")
                        {
                            rot = rotation.left;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                            ConstrainToVerticalSurface();
                        }
                        else if (LeftRaycast.collider.gameObject.tag == "Inner Platform Corner")
                        {
                            Debug.Log("Corner Hit by Left Raycast");
                            rot = rotation.left;
                            rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                            ConstrainToVerticalSurface();
                        }
                    }
                    else
                    {
                        rot = rotation.up;
                        rb.constraints = RigidbodyConstraints2D.None;
                    }
                }
            }
        }
    }

    private void ConstrainToHorizontalSurface()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void ConstrainToVerticalSurface()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void DetachFromPlatform()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        loc = location.in_air;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Outer Platform")
        {
            loc = location.outer;
        }

        if (collision.gameObject.tag == "Inner Platform")
        {
            loc = location.inner;
        }

        collider = collision.gameObject;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //loc = location.in_air;
    }

    public GameObject getCollider()
    {
        return collider;
    }

    public void sameSurface()
    {
        //rb.velocity = Vector2.zero;
        if (rotated_up && Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical, layer_mask))
        {
            Debug.Log("hi");
            rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            //Debug.Log(player.GetAxis("Left Horizontal"));
            //Debug.Log(rb.velocity);
            //Debug.Log("hi");
        }
    }

    public void find_grapple_collision(GameObject collision)
    {
        grapple_collision = collision;
    }

    public GameObject get_grapple_collision()
    {
        return grapple_collision;
    }

    public string getLocation()
    {
        if (loc == location.in_air)
        {
            return "air";
        }

        return "";
    }
}