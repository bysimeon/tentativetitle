using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private bool planted;
    Quaternion rotation;
    Vector3 position;
    Transform plantedTransform;


    // These two methods make sure that if the hook is planted, it
    // does not rotate around the Player's rotation (they stay in place
    // despite being children of Player).

    private void LateUpdate()
    {
        if(planted)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Outer Platform" ||
            collision.gameObject.tag == "Inner Platform" &&
            planted == false)
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

        if(collision.gameObject.tag == "Player 1" & transform.parent.gameObject != collision.gameObject)
        {
            Debug.Log(transform.parent.gameObject);
            Destroy(gameObject);
            var damage = GameObject.Find("Damage_Manager");
            damage.GetComponent<Damage_Player>().IncrementP1();
        }

        if (collision.gameObject.tag == "Player 2" & transform.parent.gameObject != collision.gameObject)
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
        Debug.Log("Fired");
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

        if (planted && Vector3.Magnitude(HookPosition-PlayerPosition) < 10f)
        {
            Debug.Log(Vector3.Magnitude(HookPosition - PlayerPosition));
            Destroy(gameObject);
        }
    }
}
