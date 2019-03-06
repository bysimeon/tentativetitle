using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_3_button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.FindGameObjectWithTag("Num_Player").GetComponent<Num_Player_Manager>().player_3 = true;
        GameObject.FindGameObjectWithTag("Num_Player").GetComponent<Num_Player_Manager>().player_2 = false;
        SceneManager.LoadScene("Stage Select");
    }
}
