using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLauncher : MonoBehaviour
{
    public const float LaunchCooldown = 0.5f;
    public const float LaunchVelocity = 3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Right Horizontal");
        float Vertical = Input.GetAxis("Right Vertical");
        Debug.Log(Vertical);
        float angle = Mathf.Atan2(Vertical, Horizontal);

        GameObject Hook = FindObjectOfType<GrapplingHook>().gameObject;
        var HookBody = Hook.GetComponent<Rigidbody2D>();
        HookBody.MoveRotation(Mathf.Rad2Deg * angle - 135);
    }
}
