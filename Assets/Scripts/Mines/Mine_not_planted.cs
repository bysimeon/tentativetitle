using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_not_planted : MonoBehaviour
{
    private GameObject player_hit;

    private Coroutine routine;
    private GameObject prior_object;

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
        if(collision.gameObject.tag == "Inner Platform")
        {
            Debug.Log("hi");
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 8)
        {
            player_hit = collision.gameObject;
            player_hit.GetComponentInChildren<Mine_Manager>().getMine();
            Destroy(gameObject);
        }
    }
}
