using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private bool planted;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Outer Platform" ||
            collision.gameObject.tag == "Inner Platform")
        {
            Outer_Movement ShooterMovement = transform.parent.
                gameObject.GetComponent<Outer_Movement>();
            Inner_Movement ShooterMovement2 = transform.parent.
                gameObject.GetComponent<Inner_Movement>();
            ShooterMovement.DetachFromPlatform();
            ShooterMovement2.DetachFromPlatform();
            PlantHook();
            DragPlayerToHook();

        }

    }
    void PlantHook()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeAll;
        planted = true;
    }
    // TODO: Clean this up
    void DragPlayerToHook()
    {
        GameObject Player = transform.parent.gameObject;
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Player.transform.position;
        Vector3 PlayerVelocity = Vector3.Normalize(HookPosition - PlayerPosition) *
                                    20f;
        Debug.Log(PlayerVelocity);
        Player.GetComponent<Rigidbody2D>().velocity = PlayerVelocity;
    }
    void Update()
    {
        GameObject Player = transform.parent.gameObject;
        Vector3 HookPosition = transform.position;
        Vector3 PlayerPosition = Player.transform.position;
        if(planted && Vector3.Magnitude(HookPosition-PlayerPosition) < 10f)
        {
            Debug.Log(Vector3.Magnitude(HookPosition - PlayerPosition));
            Destroy(gameObject);
        }
    }
}
