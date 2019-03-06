using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_planted : MonoBehaviour
{
    public int playerId;
    private static Object minePrefab;
    public AudioClip explode;

    // Start is called before the first frame update
    void Start()
    {
        minePrefab = Resources.Load("Prefabs/MineExplosion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.layer == 8)
        {
            int collision_playerId = collision.gameObject.GetComponent<Movement>().playerId;
            if (playerId != collision_playerId)
            {
                Shields shield = collision.gameObject.GetComponentInChildren<Shields>();
                var damageManager = GameObject.Find("Damage_Manager");

                Color playerColor = shield.getPlayerColor();
                GameObject explodeEffect = (GameObject)Instantiate(minePrefab,
                                                       collision.transform.position,
                                                       collision.transform.rotation);

                var shieldParticleSystem = explodeEffect.transform.Find("ShieldParticles").
                    GetComponent<ParticleSystem>().main;
                float red = playerColor.r;
                float green = playerColor.g;
                float blue = playerColor.b;
                Color shieldColor = new Color(red / 255, green / 255, blue / 255);

                shieldParticleSystem.startColor = new ParticleSystem.MinMaxGradient(shieldColor);

                shield.TakeDamage(50);
                shield.ShowShieldDamage(null, null);
                Destroy(gameObject);
            }
        }
    }
}
