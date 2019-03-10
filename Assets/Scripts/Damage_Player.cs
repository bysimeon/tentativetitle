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

    private float camShakeMagnitude;
    public float initialChamShakeMagnitude = 30.0f;
    private Object NukePrefab;

    private float CameraX;
    private float CameraY;
    private GameObject Camera;

    public Canvas canvas;

    private Color flashColor;
    private Color textColor;

    public Text win_text;

    public Scene_Manager scene;
    private Scene_Manager scene_script;

    public AudioClip hit;
    public AudioClip win;
    private bool cameraShaking;
    public int alivePlayers;

    public bool p1_alive = true;
    public bool p2_alive = true;
    public bool p3_alive = true;

    public Text p1_win;
    public Text p2_win;
    public Text p3_win;

    // Start is called before the first frame update
    void Start () {
        p1_win.enabled = false;
        p2_win.enabled = false;
        p3_win.enabled = false;
        scene_script = scene.GetComponent<Scene_Manager> ();
        NukePrefab = Resources.Load("Prefabs/NukeExplosion");
        //flashColor = Color.red;
        //textColor = GameObject.FindWithTag("Player1Text").
        //GetComponent<Text>().color;
        cameraShaking = false;
        Camera = GameObject.Find("Main Camera");
        CameraX = Camera.transform.position.x;
        CameraY = Camera.transform.position.y;
        camShakeMagnitude = initialChamShakeMagnitude;
    }

    // Update is called once per frame
    void Update () {
        HandleCameraShakes();
    }

    void HandleCameraShakes()
    {
        if(cameraShaking)
        {
            float yOffset = camShakeMagnitude * Mathf.PerlinNoise(Time.time * 10, Time.time * 10) - camShakeMagnitude/2;
            float xOffset = camShakeMagnitude * Mathf.PerlinNoise(Time.time * 10 + 155555, Time.time * 10 +5555) - camShakeMagnitude / 2;
            Camera.transform.position = new Vector3(CameraX + xOffset, CameraY + yOffset, -10f);
            Debug.Log("Yo");
            Debug.Log(yOffset);
            Debug.Log(xOffset);
        }
        else
        {
            Camera.transform.position.Set(CameraX, CameraY, -10f);
        }
    }



    public void DamagePlayer (GameObject player, Collision2D collision, GameObject damager) {
        GetComponent<AudioSource> ().PlayOneShot (hit);
        Shields playerShields = player.GetComponentInChildren<Shields> ();

        playerShields.TakeDamage(27);
        playerShields.ShowShieldDamage (collision, damager);
        if (!playerShields.ShieldsUp ()) {

            KillPlayer(player);
            alivePlayers -= 1;
            if (alivePlayers == 1) {
                StartCoroutine (wait ());
            }
        }
    }

    private void KillPlayer(GameObject player)
    {
        Shields shield = player.GetComponentInChildren<Shields>();
        var damageManager = GameObject.Find("Damage_Manager");

        Color playerColor = shield.getPlayerColor();
        GameObject explodeEffect = (GameObject)Instantiate(NukePrefab,
                                               player.transform.position,
                                               player.transform.rotation);
        explodeEffect.transform.SetAsLastSibling();
        var shieldParticleSystem = explodeEffect.transform.Find("ShieldParticles").
            GetComponent<ParticleSystem>().main;
        float red = playerColor.r;
        float green = playerColor.g;
        float blue = playerColor.b;
        Color shieldColor = new Color(red / 255, green / 255, blue / 255);

        shieldParticleSystem.startColor = new ParticleSystem.MinMaxGradient(shieldColor);
        StartCoroutine(NukeFlash());

        if(player == player1)
        {
            p1_alive = false;
        }

        else if (player == player2)
        {
            p2_alive = false;
        }

        else if (player == player3)
        {
            p3_alive = false;
        }
        Destroy(player.gameObject);
    }
    IEnumerator NukeFlash()
    {
        cameraShaking = true;
        for(float i = 0; i <= 1.0f; i= i +0.2f)
        {
            GameObject canvas = GameObject.Find("Canvas");
            canvas.GetComponent<AudioSource>().Play();
            canvas.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);
        
       
        for (float i = 1; i >= 0f; i -= 0.01f)
        {
            GameObject canvas = GameObject.Find("Canvas");
            canvas.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.03f);
            camShakeMagnitude = initialChamShakeMagnitude * i;
        }
        cameraShaking = false;
        camShakeMagnitude = initialChamShakeMagnitude;
    }

    IEnumerator wait () {
        if (p1_alive)
        {
            p1_win.enabled = true;
        }

        if (p2_alive)
        {
            p2_win.enabled = true;
        }

        if (p3_alive)
        {
            p3_win.enabled = true;
        }

        GameObject.FindGameObjectWithTag ("Music").GetComponent<Music_Manager> ().StopMusic ();
        GetComponent<AudioSource> ().PlayOneShot (win, 6f);
        scene_script.can_move = false;
        yield return new WaitForSeconds (4f);
        SceneManager.LoadScene ("Stage Select");
    }
}