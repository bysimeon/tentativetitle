using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_planted : MonoBehaviour
{
    public int playerId;

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
            int collision_playerId = collision.gameObject.GetComponent<Movement>().playerId;
            if (playerId != collision_playerId)
            {
                Shields shield = collision.gameObject.GetComponentInChildren<Shields>();
                shield.TakeDamage(50);
                shield.ShowShieldDamage(null, null);
                Destroy(gameObject);
            }
        }
    }
}
