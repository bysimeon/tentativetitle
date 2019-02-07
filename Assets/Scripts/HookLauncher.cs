﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLauncher : MonoBehaviour
{
    public const float LaunchCooldown = 0.5f;
    public const float LaunchVelocity = 3f;
    private static Object HookPrefab;
    float lastShotTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        HookPrefab = Resources.Load("GrapplingHook");
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Right Horizontal");
        float Vertical = Input.GetAxis("Right Vertical");
        if (GameInput.SignificantStickInput(Vertical,Horizontal))
        {
            float AimAngle = CalculateAimAngle(Vertical, Horizontal);
            DisplayAimReticle(AimAngle);
        }
        else
        {
            HideAimReticle();
        }
    }
    private float CalculateAimAngle(float Vertical, float Horizontal)
    {
        return Mathf.Atan2(Vertical, Horizontal) * Mathf.Rad2Deg;
    }
    private void HideAimReticle()
    {
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
    private void DisplayAimReticle(float angle)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void Fire()
    {
        float time = Time.time;
        if (time > lastShotTime + LaunchCooldown)
        {
            lastShotTime = time;
            spawnHook(transform.position + transform.up * 10f,
                transform.rotation,
                transform.up * LaunchVelocity);

        }
    }
    void spawnHook(Vector2 position, Quaternion rotation, Vector2 velocity)
    {

    }
}
