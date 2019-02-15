using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage_Player : MonoBehaviour
{
    private int player1_count = 0;
    private int player2_count = 0;

    public GameObject player1;
    public GameObject player2;

    public Canvas canvas;
    public GameObject p1_heart1;
    public GameObject p1_heart2;
    public GameObject p1_heart3;

    public GameObject p2_heart1;
    public GameObject p2_heart2;
    public GameObject p2_heart3;


    private Color flashColor;
    private Color textColor;


    public Text win_text;

    // Start is called before the first frame update
    void Start()
    {
        flashColor = Color.red;
        textColor = GameObject.FindWithTag("Player1Text").
            GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (player1_count >= 3)
        {
            Destroy(player1.gameObject);
        }

        if (player2_count >= 3)
        {
            Destroy(player2.gameObject);
        }
    }
    IEnumerator Flash(GameObject damagedPlayer)
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

    public void IncrementP1()
    {
        player1_count++;
        StartCoroutine("Flash",GameObject.FindWithTag("Player1"));
        if(player1_count == 1)
        {
            Destroy(p1_heart3);
        }
        if (player1_count == 2)
        {
            Destroy(p1_heart2);
        }
        if (player1_count == 3)
        {
            Destroy(p1_heart1);
            win_text.text = "A Winner is Player 2!";
        }
    }

    public void IncrementP2()
    {
        StartCoroutine("Flash", GameObject.FindWithTag("Player2"));
        player2_count++;
        if (player2_count == 1)
        {
            Destroy(p2_heart3);
        }
        if (player2_count == 2)
        {
            Destroy(p2_heart2);
        }
        if (player2_count == 3)
        {
            Destroy(p2_heart1);
            win_text.text = "A Winner is Player 1!";
        }
    }
}
