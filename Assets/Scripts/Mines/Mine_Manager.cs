using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Mine_Manager : MonoBehaviour
{
    private bool has_mine = false;
    private Player player;
    private int playerId;

    private Object mine_planted;
    private GameObject new_mine;

    private Quaternion rotation = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        mine_planted = Resources.Load("Prefabs/mine_planted");
        //For debugging purposes
        /*
        GameObject temp = (GameObject)Instantiate(mine_planted,
                new Vector3(300, 0, 0),
                rotation);
        temp.GetComponent<Mine_planted>().playerId = 5;
        */
        //
    }

    void Awake()
    {
        playerId = GetComponentInParent<Movement>().playerId;
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        if(has_mine && player.GetButtonDown("Fire 3"))
        {
            has_mine = false;
            GetComponent<SpriteRenderer>().enabled = false;
            new_mine = (GameObject)Instantiate(mine_planted,
                            transform.parent.position,
                            rotation);
            new_mine.GetComponent<SpriteRenderer>().color =
                transform.parent.GetComponentInChildren<Shields>().GetComponent<SpriteRenderer>().color;
            new_mine.GetComponent<Mine_planted>().playerId = playerId;
            Destroy(new_mine, 15f);
        }
        
    }

    public void getMine()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        has_mine = true;
    }
}
