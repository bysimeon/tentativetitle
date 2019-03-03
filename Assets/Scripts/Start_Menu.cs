using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class Start_Menu : MonoBehaviour
{
    public Button start;
    public Button tutorial;
    public Button credits;
    public Button quit;

    public enum selection {start, tutorial, credits, quit};
    public selection select;

    private ColorBlock original_color;

    public bool start_clicked = false;
    public bool quit_clicked = false;

    public GameObject player;

    private Player special_player;

    public AudioClip switching;
    private AudioSource source;

    public Music_Manager music;
    private Music_Manager music_script;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        int playerId = player.GetComponent<Movement>().playerId;
        special_player = ReInput.players.GetPlayer(playerId);
        select = selection.start;
        start.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<Music_Manager>().PlayMusic();

        HookLauncher hook = player.GetComponentInChildren<HookLauncher>();

        if ((special_player.GetButtonDown("Fire Hook") || special_player.GetButtonDown("Fire 3"))
            && select == selection.start)
        {
            hook.startCall();
        }

        if ((special_player.GetButtonDown("Fire Hook") || special_player.GetButtonDown("Fire 3"))
            && select == selection.quit)
        {
            hook.startCall();
        }

        if (special_player.GetAxis("Left Horizontal") > .8f && select == selection.start)
        {
            select = selection.tutorial;

            start.GetComponent<Image>().color = Color.white;
            tutorial.GetComponent<Image>().color = Color.green;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Horizontal") < -.8f && select == selection.tutorial)
        {
            select = selection.start;

            start.GetComponent<Image>().color = Color.green;
            tutorial.GetComponent<Image>().color = Color.white;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Vertical") < -.8f && select == selection.start)
        {
            select = selection.credits;

            start.GetComponent<Image>().color = Color.white;
            credits.GetComponent<Image>().color = Color.green;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Vertical") > .8f && select == selection.credits)
        {
            select = selection.start;

            start.GetComponent<Image>().color = Color.green;
            credits.GetComponent<Image>().color = Color.white;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Vertical") < -.8f && select == selection.tutorial)
        {
            select = selection.quit;

            tutorial.GetComponent<Image>().color = Color.white;
            quit.GetComponent<Image>().color = Color.green;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Vertical") > .8f && select == selection.quit)
        {
            select = selection.tutorial;

            tutorial.GetComponent<Image>().color = Color.green;
            quit.GetComponent<Image>().color = Color.white;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Horizontal") < -.8f && select == selection.quit)
        {
            select = selection.credits;

            quit.GetComponent<Image>().color = Color.white;
            credits.GetComponent<Image>().color = Color.green;
            source.PlayOneShot(switching);
        }

        else if (special_player.GetAxis("Left Horizontal") > .8f && select == selection.credits)
        {
            select = selection.quit;

            quit.GetComponent<Image>().color = Color.green;
            credits.GetComponent<Image>().color = Color.white;
            source.PlayOneShot(switching);
        }
    }

}
