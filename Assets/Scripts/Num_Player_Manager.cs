using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Num_Player_Manager : MonoBehaviour
{
    public bool player_2 = false;
    public bool player_3 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
