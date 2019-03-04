using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour {
    private static Object effectPrefab;
    public float strength;
    public float working;

    public Color start_color;

    public AudioClip explode;

    public Sprite[] shieldSprites;

    // Start is called before the first frame update
    void Start () {
        strength = 100f;
        StartCoroutine (RegenerateShields ());
        effectPrefab = Resources.Load ("Prefabs/shieldHitEffect");
        shieldSprites = Resources.LoadAll<Sprite> ("shield");
        Debug.Log(shieldSprites);
    }

    // Update is called once per frame
    void Update () {
        UpdateShieldDisplay ();
    }

    IEnumerator RegenerateShields () {
        while (true) {
            if (strength < 95) {
                strength += 5f;
            } else {
                strength = 100;
            }
            yield return new WaitForSeconds (1);
        }
    }
    void UpdateShieldDisplay () {
        GameObject shieldObject = gameObject;
        var sprite = shieldObject.GetComponent<SpriteRenderer> ();
        Color color = getPlayerColor ();
        float red = color.r;
        float green = color.g;
        float blue = color.b;
        float alpha = (strength / 100f);
        Color shieldColor = new Color (red / 255, green / 255, blue / 255, alpha);
        sprite.color = shieldColor;
        if (strength < 25) {
            sprite.sprite = shieldSprites[3];
        } else if (strength < 50) {
            sprite.sprite = shieldSprites[2];
        } else if (strength < 75) {
            sprite.sprite = shieldSprites[1];
        } else if (strength < 100) {
            sprite.sprite = shieldSprites[0];
        }
    }
    public void ShowShieldDamage (Collision2D collision, GameObject hook) {
        if (collision == null) {
            Color color = getPlayerColor ();
            float red = color.r;
            float green = color.g;
            float blue = color.b;
            Color shieldColor = new Color (red / 255, green / 255, blue / 255);
            GetComponent<AudioSource> ().PlayOneShot (explode);
        } else {
            Color color = getPlayerColor ();

            GameObject newEffect = (GameObject) Instantiate (effectPrefab,
                collision.GetContact (0).point,
                Quaternion.LookRotation (collision.GetContact (0).normal));
            var shieldParticleSystem = newEffect.
            GetComponent<ParticleSystem> ().main;

            float red = color.r;
            float green = color.g;
            float blue = color.b;
            Color shieldColor = new Color (red / 255, green / 255, blue / 255);

            shieldParticleSystem.startColor = new ParticleSystem.MinMaxGradient (shieldColor);
        }

    }
    private Color getPlayerColor () {
        return PlayerColors.getPlayerColor (gameObject.transform.parent.gameObject);
    }

    public void TakeDamage (float IncomingDamage) {
        strength -= IncomingDamage;
        if (strength <= 0) {
            strength = 0;
        }
    }

    public bool ShieldsUp () {
        return strength > 0;
    }

}