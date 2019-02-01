﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining the vertical and horizontal movement of the player along outer platforms (NO GRAPPLING)
public class Outer_Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float ray_distance_vertical;
    public float ray_distance_horizontal;
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

    private bool outer = false;

    // Start is called before the first frame update
    void Start()
    {
        rotated_up = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per time interval
    void FixedUpdate()
    {
        //If on outer platform
        if (outer)
        {          
            //Moving along lower platform
            if (rotated_up & Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical) &
                !(Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal))
                & !(Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal)))
            {
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Moving along right platform
            if (rotated_right & Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical) &
                !(Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal)) &
                !(Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal)))
            {
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }

            //Moving along top platform
            if (rotated_down & Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical) &
            !(Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal))
            & !(Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal)))
            {
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Moving along left platform
            if (rotated_left & Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical) &
            !(Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal))
            & !(Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal)))
            {
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }

            //Switching bottom right corner from bottom to right
            if (Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal) &
                Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical) &
                rotated_up)
            {
                rotated_up = false;
                rotated_right = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + bottom_to_right_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }

            //Switching bottom right corner from right to bottom
            else if (Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical) &
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal) &
            rotated_right)
            {
                rotated_right = false;
                rotated_up = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + right_to_bottom_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Switching top right corner from right to top
            if (rotated_right & Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_vertical) &
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal))
            {
                rotated_right = false;
                rotated_down = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + right_to_top_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Switching top right corner from top to right
            else if (rotated_down & Physics2D.Raycast(rb.position, new Vector2(1, 0), ray_distance_horizontal) &
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rotated_down = false;
                rotated_right = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + top_to_right_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }

            //Switching top left corner from left to top
            if (rotated_left & Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical) &
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_horizontal))
            {
                rotated_left = false;
                rotated_down = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + left_to_top_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Switching top left corner from top to left
            else if (rotated_down & Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal) &
            Physics2D.Raycast(rb.position, new Vector2(0, 1), ray_distance_vertical))
            {
                rotated_down = false;
                rotated_left = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + top_to_left_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }

            //Switching bottom left corner from left to bottom
            if (Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_vertical) &
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_horizontal) &
            rotated_left)
            {
                rotated_up = true;
                rotated_left = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + left_to_bottom_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), 0);
            }

            //Switching bottom left corner from bottom to left
            else if (Physics2D.Raycast(rb.position, new Vector2(-1, 0), ray_distance_horizontal) &
            Physics2D.Raycast(rb.position, new Vector2(0, -1), ray_distance_vertical) &
            rotated_up)
            {
                rotated_left = true;
                rotated_up = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.transform.position = rb.transform.position + bottom_to_left_shift;
                rb.transform.rotation = Quaternion.Euler(0, 0, 270);
                rb.velocity = speed * new Vector2(0, Input.GetAxis("Vertical"));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Outer Platform")
        {
            outer = true;
        }

        if(collision.gameObject.tag == "Inner Platform")
        {
            outer = false;
        }
    }
}