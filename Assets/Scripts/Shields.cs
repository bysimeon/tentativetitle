using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    private static Object effectPrefab;
    public float strength;
    public float working;
    // Start is called before the first frame update
    void Start()
    {
        strength = 100f;
        StartCoroutine(RegenerateShields());
        effectPrefab = Resources.Load("Prefabs/shieldHitEffect");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShieldDisplay();
 
    }

    IEnumerator RegenerateShields()
    {
        while(true)
        {
            if (strength < 95)
            {
                strength += 5f;
            }
            else
            {
                strength = 100;
            }
            yield return new WaitForSeconds(1);
        }
    }
    void UpdateShieldDisplay()
    {
        GameObject shieldObject = gameObject;
        var sprite = shieldObject.GetComponent<SpriteRenderer>();
        Color color = getPlayerColor();
        float red = color.r;
        float green = color.g;
        float blue = color.b;
        float alpha = (strength / 100f);
        Color shieldColor = new Color(red/255, green/255, blue/255, alpha);
        sprite.color = shieldColor;
    }
    public void ShowShieldDamage(Collision2D collision, GameObject hook)
    {
        Color color = getPlayerColor();


        Debug.Log(collision.GetContact(0).point);
        GameObject newEffect = (GameObject)Instantiate(effectPrefab,
                                    collision.GetContact(0).point,
                                    Quaternion.LookRotation(collision.GetContact(0).normal));
        var shieldParticleSystem = newEffect.
            GetComponent<ParticleSystem>().main;

        float red = color.r;
        float green = color.g;
        float blue = color.b;
        Color shieldColor = new Color(red/255, green/255, blue/255);


        shieldParticleSystem.startColor = new ParticleSystem.MinMaxGradient(shieldColor);

        Debug.Log(color.r);

    }
    private Color getPlayerColor()
    {
        return PlayerColors.getPlayerColor(gameObject.
            transform.parent.gameObject);
    }

    public void TakeDamage(float IncomingDamage)
    {
        strength -= IncomingDamage;
        if (strength <= 0)
        {
            strength = 0;
        }
    }

    public bool ShieldsUp()
    {
        return strength > 0;
    }

}
