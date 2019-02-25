using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

//Defining the vertical and horizontal movement of the player along inner platforms (NO GRAPPLING)
public class Inner_Movement : MonoBehaviour {
    public int playerId;
    private Player player;

    private enum location { outer, inner, in_air };
 private location loc;
 public Rigidbody2D rb;

 public float speed;
 public float ray_distance_vertical;
 public float ray_distance_horizontal;
 public float ray_distance_in_air_vertical;
 public float ray_distance_in_air_horizontal;
 private float ray_distance_in_air_1;
 private float ray_distance_in_air_2;

 public Vector3 top_to_right;
 public Vector3 right_to_top;
 public Vector3 top_to_left;
 public Vector3 left_to_top;
 public Vector3 bottom_to_left;
 public Vector3 left_to_bottom;
 public Vector3 bottom_to_right;
 public Vector3 right_to_bottom;

    public enum rotation { up, down, right, left };
    public rotation rot;

    private GameObject collider = null;
    private int layer_mask;
    public GameObject scene_manager;
    private Scene_Manager scene;

    void Awake () {
        player = ReInput.players.GetPlayer (playerId);
    }
    // Start is called before the first frame update
    void Start () {
        layer_mask = (1 << 8 | 1 << 9);
        layer_mask = ~layer_mask;
        loc = location.in_air;
        rot = rotation.up;

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per time interval
    void FixedUpdate () {
        rot = (Inner_Movement.rotation)GetComponentInParent<Outer_Movement>().rot;
        RaycastHit2D DownRaycast = new RaycastHit2D ();
        RaycastHit2D UpRaycast = new RaycastHit2D ();
        RaycastHit2D RightRaycast = new RaycastHit2D ();
        RaycastHit2D LeftRaycast = new RaycastHit2D ();

        if (rot == rotation.up | rot == rotation.down) {
            ray_distance_in_air_1 = ray_distance_in_air_vertical;
            ray_distance_in_air_2 = ray_distance_in_air_horizontal;
        } else {
            ray_distance_in_air_1 = ray_distance_in_air_horizontal;
            ray_distance_in_air_2 = ray_distance_in_air_vertical;
        }

        DownRaycast = Physics2D.Raycast (rb.position, new Vector2 (0, -1), ray_distance_in_air_1, layer_mask);
        UpRaycast = Physics2D.Raycast (rb.position, new Vector2 (0, 1), ray_distance_in_air_1, layer_mask);
        RightRaycast = Physics2D.Raycast (rb.position, new Vector2 (1, 0), ray_distance_in_air_2, layer_mask);
        LeftRaycast = Physics2D.Raycast (rb.position, new Vector2 (-1, 0), ray_distance_in_air_2, layer_mask);

        if (scene_manager != null) {
            scene = scene_manager.GetComponent<Scene_Manager> ();
        }

        if (scene != null) {
            if (scene.can_move) {
                //Attaching to platform when grappling
                if (loc == location.in_air) {
                    if (DownRaycast) {
                        if (DownRaycast.collider.gameObject.tag == "Inner Platform") {
                            rot = rotation.up;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 0);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        } else if (DownRaycast.collider.gameObject.tag == "Inner Platform Corner") {
                            Debug.Log("Corner Hit by Down Raycast");
                            rot = rotation.up;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 0);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                    } else if (
                        UpRaycast) {
                        if (UpRaycast.collider.gameObject.tag == "Inner Platform") {
                            rot = rotation.down;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 180);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        } else if (UpRaycast.collider.gameObject.tag == "Inner Platform Corner") {
                            Debug.Log("Corner Hit by Up Raycast");
                            rot = rotation.down;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 180);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                    } else if (
                        RightRaycast) {
                        if (RightRaycast.collider.gameObject.tag == "Inner Platform") {
                            rot = rotation.right;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 90);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        } else if (RightRaycast.collider.gameObject.tag == "Inner Platform Corner") {
                            Debug.Log("Corner Hit by Right Raycast");
                            rot = rotation.right;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 90);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                    } else if (
                        LeftRaycast) {
                        if (LeftRaycast.collider.gameObject.tag == "Inner Platform") {
                            rot = rotation.left;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 270);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        } else if (LeftRaycast.collider.gameObject.tag == "Inner Platform Corner") {
                            Debug.Log("Corner Hit by Left Raycast");
                            rot = rotation.left;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 270);
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                    }
                    else
                    {
                        rot = rotation.up;
                        rb.constraints = RigidbodyConstraints2D.None;
                    }
                }

                //If on inner platform
                if (loc == location.inner) {

                    //Moving along top platform
                    if (rot == rotation.up & Physics2D.Raycast (rb.position, new Vector2 (0, -1), ray_distance_vertical, layer_mask)) {
                        rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                    }

                    //Moving along right platform
                    if (rot == rotation.right & Physics2D.Raycast (rb.position, new Vector2 (-1, 0), ray_distance_vertical, layer_mask)) {
                        rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                    }

                    //Moving along left platform
                    if (rot == rotation.left & Physics2D.Raycast (rb.position, new Vector2 (1, 0), ray_distance_vertical, layer_mask)) {
                        rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                    }

                    //Moving along bottom platform
                    if (rot == rotation.down & Physics2D.Raycast (rb.position, new Vector2 (0, 1), ray_distance_vertical, layer_mask)) {
                        rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                    }

                    //Switching top right corner from top to right
                    if (rot == rotation.up && !(Physics2D.Raycast (rb.position + new Vector2 (2, 0), new Vector2 (0, -1), ray_distance_vertical, layer_mask)) &&
                        Physics2D.Raycast (rb.position + new Vector2 (0, -5), new Vector2 (-1, 0), ray_distance_horizontal, layer_mask)) {
                        {
                            rot = rotation.right;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + top_to_right;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 270);
                            rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                        }
                    }

                    //Switching top right corner from right to top
                    else if (rot == rotation.right & !(Physics2D.Raycast (rb.position + new Vector2 (0, 2), new Vector2 (-1, 0), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (-5, 0), new Vector2 (0, -1), ray_distance_vertical, layer_mask)) {
                        {
                            rot = rotation.up;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + right_to_top;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 0);
                            rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                        }
                    }

                    //Switching top left corner from top to left
                    if (rot == rotation.up &
                        !(Physics2D.Raycast (rb.position + new Vector2 (-2, 0), new Vector2 (0, -1), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (0, -5), new Vector2 (1, 0), ray_distance_horizontal, layer_mask)) {
                        {
                            Debug.Log ("Switching top left corner from top to left");
                            rot = rotation.left;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + top_to_left;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 90);
                            rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                        }
                    }

                    //Switching top left corner from left to top
                    else if (rot == rotation.left & !(Physics2D.Raycast (rb.position + new Vector2 (0, 2), new Vector2 (1, 0), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (5, 0), new Vector2 (0, -1), ray_distance_vertical, layer_mask)) {
                        {
                            Debug.Log ("Switching top left corner from left to top");
                            rot = rotation.up;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + left_to_top;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 0);
                            rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                        }
                    }

                    //Switching bottom left corner from bottom to left
                    if (rot == rotation.down & !(Physics2D.Raycast (rb.position + new Vector2 (-2, 0), new Vector2 (0, 1), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (0, 5), new Vector2 (1, 0), ray_distance_horizontal, layer_mask)) {
                        {
                            Debug.Log ("Switching bottom left corner from bottom to left");
                            rot = rotation.left;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + bottom_to_left;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 90);
                            rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                        }
                    }

                    //Switching bottom left corner from left to bottom
                    else if (rot == rotation.left & !(Physics2D.Raycast (rb.position + new Vector2 (0, -2), new Vector2 (1, 0), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (5, 0), new Vector2 (0, 1), ray_distance_vertical, layer_mask)) {
                        {
                            Debug.Log ("Switching bottom left corner from left to bottom");
                            rot = rotation.down;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + left_to_bottom;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 180);
                            rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                        }
                    }

                    //Switching bottom right corner from bottom to right
                    if (rot == rotation.down & !(Physics2D.Raycast (rb.position + new Vector2 (2, 0), new Vector2 (0, 1), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (0, 5), new Vector2 (-1, 0), ray_distance_horizontal, layer_mask)) {
                        {
                            rot = rotation.right;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + bottom_to_right;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 270);
                            rb.velocity = speed * new Vector2 (0, player.GetAxis ("Left Vertical"));
                        }
                    }

                    //Switching bottom right corner from right to bottom
                    else if (rot == rotation.right & !(Physics2D.Raycast (rb.position + new Vector2 (0, -2), new Vector2 (-1, 0), ray_distance_vertical, layer_mask)) &
                        Physics2D.Raycast (rb.position + new Vector2 (-5, 0), new Vector2 (0, 1), ray_distance_vertical, layer_mask)) {
                        {
                            rot = rotation.down;
                            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            rb.transform.position = rb.transform.position + right_to_bottom;
                            rb.transform.rotation = Quaternion.Euler (0, 0, 180);
                            rb.velocity = speed * new Vector2 (player.GetAxis ("Left Horizontal"), 0);
                        }
                    }
                }
            }
        }
    }

    public void DetachFromPlatform () {
        rb.constraints = RigidbodyConstraints2D.None;
        loc = location.in_air;
    }

    private void UpdateLocation(rotation r)
    {
        GetComponentInParent<Outer_Movement>().rot = (Outer_Movement.rotation)r;
    }

    private void ConstrainToHorizontalSurface () {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void ConstrainToVerticalSurface () {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Outer Platform") {
            loc = location.outer;
        }

        if (collision.gameObject.tag == "Inner Platform") {
            loc = location.inner;
        }

        collider = collision.gameObject;
    }

    public GameObject getCollider () {
        return collider;
    }
}