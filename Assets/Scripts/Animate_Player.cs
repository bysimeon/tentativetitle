using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Animate_Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anm;
    SpriteRenderer rndr;

    public int playerId;
    private Player player;

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
        rndr = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetAxis("Left Horizontal") > 0)
        {
            anm.Play("Running");
            rndr.flipX = false;
        }
        if (player.GetAxis("Left Horizontal") < 0)
        {
            anm.Play("Running");
            rndr.flipX = true;
        }
        if (player.GetAxis("Left Vertical") > 0)
        {
            anm.Play("Running");
        }
        if (player.GetAxis("Left Vertical") < 0)
        {
            anm.Play("Running");
        }
        if (Mathf.Approximately(player.GetAxis("Left Vertical"), 0.0f) && Mathf.Approximately(player.GetAxis("Left Horizontal"), 0.0f))
        {
            anm.Play("Idle");
        }
    }
}
