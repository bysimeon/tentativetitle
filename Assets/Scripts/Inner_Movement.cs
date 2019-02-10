﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining the vertical and horizontal movement of the player along inner platforms (NO GRAPPLING)
public class Inner_Movement : MonoBehaviour
{
    private enum location { outer, inner, in_air };
    private location loc;
    public Rigidbody2D rb;

    public float speed;
    public float ray_distance_vertical;
    public float ray_distance_horizontal;
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
        //If on inner platform
        if(loc == location.inner)
        {
            //Moving along top platform
            if (rotated_up & Physics2D.Raycast(rb.position, new Vector2(0,-1), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
            }

            //Moving along right platform
            if (rotated_right & Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
            }

            //Moving along left platform
            if (rotated_left & Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
            }

            //Moving along bottom platform
            if (rotated_down & Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
            }

            //Switching top right corner from top to right
            if (rotated_up & !(Physics2D.Raycast(rb.position + new Vector2(2,0), new Vector2(0, -1), ray_distance_vertical))
                & Physics2D.Raycast(rb.position + new Vector2(0,-5), new Vector2(-1, 0), ray_distance_horizontal))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_up = false;
                    rotated_right = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + top_to_right;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                    rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                }

                else
                {
                    if (Input.GetAxis("Left Horizontal") > 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                    }
                }
            }

            //Switching top right corner from right to top
            else if (rotated_right & !(Physics2D.Raycast(rb.position + new Vector2(0,2), new Vector2(-1, 0), ray_distance_vertical))
                & Physics2D.Raycast(rb.position + new Vector2(-5, 0), new Vector2(0, -1), ray_distance_vertical))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_right = false;
                    rotated_up = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + right_to_top;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                }

                else
                {
                    if (Input.GetAxis("Left Vertical") > 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                    }
                }
            }

            //Switching top left corner from top to left
            if (rotated_up & !(Physics2D.Raycast(rb.position + new Vector2(-2,0), new Vector2(0, -1), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(0, -5), new Vector2(1, 0), ray_distance_horizontal))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_up = false;
                    rotated_left = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + top_to_left;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                    rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                }

                else
                {
                    if (Input.GetAxis("Left Horizontal") < 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                    }
                }
            }

            //Switching top left corner from left to top
            else if (rotated_left & !(Physics2D.Raycast(rb.position + new Vector2(0,2), new Vector2(1, 0), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(5, 0), new Vector2(0, -1), ray_distance_vertical))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_left = false;
                    rotated_up = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + left_to_top;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                }

                else
                {
                    if (Input.GetAxis("Left Vertical") > 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                    }
                }
            }

            //Switching bottom left corner from bottom to left
            if (rotated_down & !(Physics2D.Raycast(rb.position + new Vector2(-2,0), new Vector2(0, 1), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(0, 5), new Vector2(1, 0), ray_distance_horizontal))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_down = false;
                    rotated_left = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + bottom_to_left;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                    rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                }

                else
                {
                    if (Input.GetAxis("Left Horizontal") < 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                    }
                }
            }

            //Switching bottom left corner from left to bottom
            else if (rotated_left & !(Physics2D.Raycast(rb.position + new Vector2(0,-2), new Vector2(1, 0), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(5, 0), new Vector2(0, 1), ray_distance_vertical))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_left = false;
                    rotated_down = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + left_to_bottom;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                    rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                }

                else
                {
                    if (Input.GetAxis("Left Vertical") < 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                    }
                }
            }

            //Switching bottom right corner from bottom to right
            if (rotated_down & !(Physics2D.Raycast(rb.position + new Vector2(2,0), new Vector2(0, 1), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(0, 5), new Vector2(-1, 0), ray_distance_horizontal))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_down = false;
                    rotated_right = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + bottom_to_right;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                    rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                }

                else
                {
                    if (Input.GetAxis("Left Horizontal") > 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                    }
                }
            }

            //Switching bottom right corner from right to bottom
            else if (rotated_right & !(Physics2D.Raycast(rb.position + new Vector2(0,-2), new Vector2(-1, 0), ray_distance_vertical))
            & Physics2D.Raycast(rb.position + new Vector2(-5, 0), new Vector2(0, 1), ray_distance_vertical))
            {
                if (Input.GetAxis("Switch Platforms") > 0)
                {
                    rotated_right = false;
                    rotated_down = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.transform.position = rb.transform.position + right_to_bottom;
                    rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                    rb.velocity = speed * new Vector2(Input.GetAxis("Left Horizontal"), 0);
                }

                else
                {
                    if (Input.GetAxis("Left Vertical") < 0)
                    {
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        rb.velocity = speed * new Vector2(0, Input.GetAxis("Left Vertical"));
                    }
                }
            }
        }
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
    }
}
