using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLauncher : MonoBehaviour
{
    public const float LaunchCooldown = 3f;
    public const float LaunchVelocity = 40f;
    private static Object HookPrefab;
    float lastShotTime = 0;
    private bool IsAiming = false;
    // Start is called before the first frame update
    void Start()
    {
        HookPrefab = Resources.Load("Prefabs/Hook");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        ProcessShooting();
    }
    void UpdateAim()
    {
        float Horizontal = Input.GetAxis("Right Horizontal");
        float Vertical = Input.GetAxis("Right Vertical");
        IsAiming = GameInput.SignificantStickInput(Vertical, Horizontal);
        if (IsAiming)
        {
            float AimAngle = CalculateAimAngle(Vertical, Horizontal);
            DisplayAimReticle(AimAngle);
        }
        else
        {
            HideAimReticle();
        }
    }
    void ProcessShooting()
    {
        bool Shoot = Input.GetButtonDown("Fire Hook");
        if(Shoot)
        {
            AttemptFire();
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
    void AttemptFire()
    {
        float time = Time.time;
        if (time > lastShotTime + LaunchCooldown && IsAiming)
        {
            lastShotTime = time;

            Debug.Log(transform.rotation.z);
            SpawnHook(transform.position + transform.right * 5f,
                transform.rotation,
                transform.right * LaunchVelocity);

        }
    }
    void SpawnHook(Vector2 position, Quaternion rotation, Vector2 velocity)
    {
        Transform playerTransform = transform.parent;
        GameObject newHook = (GameObject)Instantiate(HookPrefab,
                                            position,
                                            rotation,
                                            playerTransform);
        newHook.GetComponent<Rigidbody2D>().velocity = velocity;

    }
}
