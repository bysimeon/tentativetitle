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


    private Object NukePrefab;

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

    public void DamagePlayer (GameObject player, Collision2D collision, GameObject damager) {
        GetComponent<AudioSource> ().PlayOneShot (hit);
        Shields playerShields = player.GetComponentInChildren<Shields> ();

        playerShields.TakeDamage(27);
        playerShields.ShowShieldDamage (collision, damager);
        if (!playerShields.ShieldsUp ()) {

            KillPlayer(player);
            Destroy (player.gameObject);
            alivePlayers -= 1;
            if (alivePlayers == 1) {
                StartCoroutine (wait ());
            }
        }
    }

    private void KillPlayer(GameObject player)
    {

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