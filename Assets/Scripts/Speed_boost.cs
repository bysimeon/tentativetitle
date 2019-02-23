using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_boost : MonoBehaviour
{
    private GameObject player_hit;
    public float fast_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            player_hit = collision.gameObject;
            StartCoroutine(speed_up());
        }
    }

    IEnumerator speed_up()
    {
        HookLauncher launcher = player_hit.GetComponentInChildren<HookLauncher>();
        launcher.LaunchVelocity = fast_speed;
        launcher.travel_speed = fast_speed;
        yield return new WaitForSeconds(5f);
        launcher.LaunchVelocity = 150f;
        launcher.travel_speed = 150f;
        Destroy(gameObject);
    }
}
