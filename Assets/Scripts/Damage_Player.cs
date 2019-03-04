using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Damage_Player : MonoBehaviour {
    private int player1_count = 0;
    private int player2_count = 0;
    private int player3_count = 0;
    private int player4_count = 0;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public Canvas canvas;

    private Color flashColor;
    private Color textColor;

    public Text win_text;

    public Scene_Manager scene;
    private Scene_Manager scene_script;

    public AudioClip hit;
    public AudioClip win;

    public int alivePlayers;

    // Start is called before the first frame update
    void Start () {
        scene_script = scene.GetComponent<Scene_Manager> ();
        //flashColor = Color.red;
        //textColor = GameObject.FindWithTag("Player1Text").
        //GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update () {
        if (player1_count >= 3) {
            Destroy (player1.gameObject);
        }

        if (player2_count >= 3) {
            Destroy (player2.gameObject);
        }
        if (player3_count >= 3) {
            Destroy (player3.gameObject);
        }
        if (player4_count >= 3) {
            Destroy (player4.gameObject);
        }
    }
    /*IEnumerator Flash(GameObject damagedPlayer)
    {
        string playerTag = damagedPlayer.tag;
        GameObject text = GameObject.FindWithTag(playerTag + "Text");
        for (float f = 0f; f < 1.15f; f += 0.15f)
        {
            // check to see if the Player's destroyed
            if(damagedPlayer)
            {
                Color currentColor = text.GetComponent<Text>().color;
                text.GetComponent<Text>().color = currentColor.Equals(textColor) ?
                    flashColor :
                    textColor;
            }

            yield return new WaitForSeconds(.15f);
        }
    }
    */

    public void DamagePlayer (GameObject player, Collision2D collision, GameObject hook) {
        GetComponent<AudioSource> ().PlayOneShot (hit);
        Shields playerShields = player.GetComponentInChildren<Shields> ();
        playerShields.TakeDamage (25);
        playerShields.ShowShieldDamage (collision, hook);
        if (!playerShields.ShieldsUp ()) {
            Destroy (player.gameObject);
            alivePlayers -= 1;
            if (alivePlayers == 1) {
                StartCoroutine (wait ());
            }
        }
    }

    IEnumerator wait () {
        win_text.text = "GAME!";
        GameObject.FindGameObjectWithTag ("Music").GetComponent<Music_Manager> ().StopMusic ();
        GetComponent<AudioSource> ().PlayOneShot (win, 6f);
        scene_script.can_move = false;
        yield return new WaitForSeconds (4f);
        SceneManager.LoadScene ("Stage Select");
    }
}