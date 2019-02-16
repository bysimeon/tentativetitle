using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

//Defining the vertical and horizontal movement of the player along outer platforms (NO GRAPPLING)
public class Outer_Movement : MonoBehaviour
{
    public int playerId;
    private Player player;

    public enum location { outer, inner, in_air };
    public location loc;
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

    private bool rotated_up = false;
    private bool rotated_right = false;
    private bool rotated_left = false;
    private bool rotated_down = false;

    private GameObject collider = null;
    private GameObject grapple_collision = null;

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
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

        if (rotated_up | rotated_down)
        {
            ray_distance_in_air_1 = ray_distance_in_air_vertical;
            ray_distance_in_air_2 = ray_distance_in_air_horizontal;
        }

        else
        {
            ray_distance_in_air_1 = ray_distance_in_air_horizontal;
            ray_distance_in_air_2 = ray_distance_in_air_vertical;
        }

        DownRaycast = Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_in_air_1);
        UpRaycast = Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_in_air_1);
        RightRaycast = Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_in_air_2);
        LeftRaycast = Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_in_air_2);

        /*
        if (DownRaycast)
        {
            Debug.Log("Down" + DownRaycast.transform.gameObject);
        }
        if (UpRaycast)
        {
            Debug.Log("Up" + UpRaycast.transform.gameObject);
        }
        if (RightRaycast)
        {
            Debug.Log("Right" + RightRaycast.transform.gameObject);
        }
        if (LeftRaycast)
        {
            Debug.Log("Left" + LeftRaycast.transform.gameObject);
        }
        */

        //Attaching to platform when grappling
        if (loc == location.in_air &&
            DownRaycast && !rotated_up &&
            DownRaycast.collider.gameObject.tag == "Outer Platform")
        {
            rotated_up = true;
            rotated_right = false;
            rotated_left = false;
            rotated_down = false;
            rb.transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        else if (loc == location.in_air &&
            UpRaycast && !rotated_down &&
            UpRaycast.collider.gameObject.tag == "Outer Platform")
        {
            rotated_down = true;
            rotated_right = false;
            rotated_left = false;
            rotated_up = false;
            rb.transform.rotation = Quaternion.Euler(0, 0, 180);
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        else if (loc == location.in_air &&
            RightRaycast && !rotated_right &&
            RightRaycast.collider.gameObject.tag == "Outer Platform")
        {
            rotated_right = true;
            rotated_up = false;
            rotated_left = false;
            rotated_down = false;
            rb.transform.rotation = Quaternion.Euler(0, 0, 90);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        else if (loc == location.in_air &&
            LeftRaycast && !rotated_left &&
            LeftRaycast.collider.gameObject.tag == "Outer Platform")
        {
            rotated_left = true;
            rotated_right = false;
            rotated_up = false;
            rotated_down = false;
            rb.transform.rotation = Quaternion.Euler(0, 0, 270);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //If on outer platform
        if (loc == location.outer)
        {
            //Moving along lower platform
            if (rotated_up && Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
                //Debug.Log(player.GetAxis("Left Horizontal"));
                //Debug.Log(rb.velocity);
                //Debug.Log("hi");
            }

            //Moving along right platform
            if (rotated_right && Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
            }

            //Moving along top platform
            if (rotated_down && Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            }

            //Moving along left platform
            if (rotated_left && Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
            }

            //Switching bottom right corner from bottom to right
            if (Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal) &&
                Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical) &&
                rotated_up)
            {
                rotated_up = false;
                rotated_right = true;
                ConstrainToVerticalSurface();
                rb.transform.position = rb.transform.position + bottom_to_right_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
            }

            //Switching bottom right corner from right to bottom
            else if (Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical) &&
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal) &&
            rotated_right)
            {
                rotated_right = false;
                rotated_up = true;
                ConstrainToHorizontalSurface();
                rb.transform.position = rb.transform.position + right_to_bottom_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            }

            //Switching top right corner from right to top
            if (rotated_right && Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical) &&
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal))
            {
                rotated_right = false;
                rotated_down = true;
                ConstrainToHorizontalSurface();
                rb.transform.position = rb.transform.position + right_to_top_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            }

            //Switching top right corner from top to right
            else if (rotated_down && Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal) &&
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rotated_down = false;
                rotated_right = true;
                ConstrainToVerticalSurface();
                rb.transform.position = rb.transform.position + top_to_right_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
            }

            //Switching top left corner from left to top
            if (rotated_left && Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical) &&
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal))
            {
                rotated_left = false;
                rotated_down = true;
                ConstrainToHorizontalSurface();
                rb.transform.position = rb.transform.position + left_to_top_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            }

            //Switching top left corner from top to left
            else if (rotated_down && Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal) &&
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rotated_down = false;
                rotated_left = true;
                ConstrainToVerticalSurface();
                rb.transform.position = rb.transform.position + top_to_left_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
            }

            //Switching bottom left corner from left to bottom
            if (Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical) &&
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal) &&
            rotated_left)
            {
                rotated_up = true;
                rotated_left = false;
                ConstrainToHorizontalSurface();
                rb.transform.position = rb.transform.position + left_to_bottom_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = speed * new Vector2(player.GetAxis("Left Horizontal"), 0);
            }

            //Switching bottom left corner from bottom to left
            else if (Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal) &&
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical) &&
            rotated_up)
            {
                rotated_left = true;
                rotated_up = false;
                ConstrainToVerticalSurface();
                rb.transform.position = rb.transform.position + bottom_to_left_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                rb.velocity = speed * new Vector2(0, player.GetAxis("Left Vertical"));
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

    public GameObject getCollider()
    {
        return collider;
    }

    public void sameSurface()
    {
        //rb.velocity = Vector2.zero;
        if (rotated_up && Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical))
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
        if(loc == location.in_air)
        {
            return "air";
        }

        return "";
    }
}
