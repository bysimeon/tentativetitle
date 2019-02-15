﻿using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private bool planted;
    Quaternion rotation;
    Vector3 position;
    Transform plantedtransform;
    public GameObject Player;
    public float ray_distance;
    public bool same_surface = false;

    void LateUpdate()
    {
        if(planted)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Outer_Movement outer_collision = Player.GetComponent<Outer_Movement>();
        Inner_Movement inner_collision = Player.GetComponent<Inner_Movement>();

        if (collision.gameObject == outer_collision.getCollider() |
            collision.gameObject == inner_collision.getCollider())
        {
            same_surface = true;
        }

        else
        {
            same_surface = false;
        }

        if ((collision.gameObject.tag == "Outer Platform" ||
            collision.gameObject.tag == "Inner Platform") &&
            planted == false)
        {
            Outer_Movement ShooterMovement = Player.
                GetComponent<Outer_Movement>();
            Inner_Movement ShooterMovement2 = Player.
                GetComponent<Inner_Movement>();
            ShooterMovement.DetachFromPlatform();
            ShooterMovement2.DetachFromPlatform();
            PlantHook();
            DragPlayerToHook();
        }

        if(collision.gameObject.tag == "Player1" & Player != collision.gameObject)
        {
            Destroy(gameObject);
            var damage = GameObject.Find("Damage_Manager");
            damage.GetComponent<Damage_Player>().IncrementP1();
        }

        if (collision.gameObject.tag == "Player2" & Player != collision.gameObject)
        {
            Destroy(gameObject);
            var damage = GameObject.Find("Damage_Manager");
            damage.GetComponent<Damage_Player>().IncrementP2();
        }
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
    void DragPlayerToHook()
    {
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 PlayerVelocity = Vector3.Normalize(HookPosition - PlayerPosition) *
                                        50f;
        Player.GetComponent<Rigidbody2D>().velocity = PlayerVelocity;

    }
    void Update()
    {
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Player.transform.position;

        if (planted && Vector3.Magnitude(HookPosition - PlayerPosition) < 10f)
        {
            Destroy(gameObject);
        }
    }
}
