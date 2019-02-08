using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Outer Platform" ||
            collision.gameObject.tag == "Inner Platform")
        {
            Outer_Movement ShooterMovement = transform.parent.
                gameObject.GetComponent<Outer_Movement>();
            ShooterMovement.DetachFromPlatform();
            PlantHook();
            DragPlayerToHook();
        }

    }
    void PlantHook()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeAll;
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
}
