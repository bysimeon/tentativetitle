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
        if (Mathf.Abs(Horizontal) + Mathf.Abs(Vertical) > 0.3f)
        {
            float angle = Mathf.Atan2(Vertical, Horizontal) * Mathf.Rad2Deg;
            DisplayAimReticle(angle);
        }
        else
        {
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }
    void DisplayAimReticle(float angle)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
