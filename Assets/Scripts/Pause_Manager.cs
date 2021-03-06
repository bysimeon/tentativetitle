﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Manager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    private Player player1_new;
    private Player player2_new;
    private Player player3_new;

    public bool paused = false;

    public Text pause_text;
    public Text fight_text;

    public Scene_Manager scene;
    private Scene_Manager scene_script;

    public AudioClip pause;

    // Start is called before the first frame update
    void Start()
    {
        int playerId = player1.GetComponent<Movement>().playerId;
        int player2Id = player2.GetComponent<Movement>().playerId;
        int player3Id = player3.GetComponent<Movement>().playerId;
        player1_new = ReInput.players.GetPlayer(playerId);
        player2_new = ReInput.players.GetPlayer(player2Id);
        player3_new = ReInput.players.GetPlayer(player3Id);
        pause_text.enabled = false;
        scene_script = scene.GetComponent<Scene_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if((player1_new.GetButtonDown("Start") || player2_new.GetButtonDown("Start") 
            || player3_new.GetButtonDown("Start")) && !paused && scene_script.can_move)
        {
            GetComponent<AudioSource>().PlayOneShot(pause);
            Time.timeScale = 0f;
            paused = true;
            fight_text.enabled = false;
            pause_text.enabled = true;
        }

        else if ((player1_new.GetButtonDown("Back") || player2_new.GetButtonDown("Back")
            || player3_new.GetButtonDown("Back")) && paused && scene_script.can_move)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<Music_Manager>().StopMusic();
            GameObject.FindGameObjectWithTag("Music").GetComponent<Music_Manager>().PlayMusic();
            SceneManager.LoadScene("Stage Select");
            Time.timeScale = 1f;
            paused = false;
            pause_text.enabled = false;
        }

        else if ((player1_new.GetButtonDown("Start") || player2_new.GetButtonDown("Start")
            || player3_new.GetButtonDown("Start")) && paused && scene_script.can_move)
        {
            Time.timeScale = 1f;
            paused = false;
            pause_text.enabled = false;
        }

    }
}
