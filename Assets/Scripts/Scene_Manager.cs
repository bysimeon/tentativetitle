using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
    public Text count3;
    public Text count2;
    public Text count1;
    public Text fight;

    public bool can_move = false;

    // Start is called before the first frame update
    void Start()
    {
        count3.enabled = false;
        count2.enabled = false;
        count1.enabled = false;
        fight.enabled = false;
        Scene currentScene = SceneManager.GetActiveScene();
        string scene_name = currentScene.name;
        if(scene_name == "Stage 1")
        {
            StartCoroutine(countdown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countdown()
    {
        count3.enabled = true;
        yield return new WaitForSeconds(1f);
        count3.enabled = false;
        count2.enabled = true;
        yield return new WaitForSeconds(1f);
        count2.enabled = false;
        count1.enabled = true;
        yield return new WaitForSeconds(1f);
        count1.enabled = false;
        fight.enabled = true;
        can_move = true;
        yield return new WaitForSeconds(1f);
        fight.enabled = false;
    }
}
