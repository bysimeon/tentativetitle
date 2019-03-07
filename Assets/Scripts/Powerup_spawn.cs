using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_spawn : MonoBehaviour
{
    public Scene_Manager scene;
    private Scene_Manager scene_script;
    private Object speed_boost;
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);
    private Vector2 position = new Vector2(304, 0);

    public float y_max;
    public float y_min;

    public float x_max;
    public float x_min;

    private GameObject newItem;
    private Coroutine routine;

    private Object mine;

    // Start is called before the first frame update
    void Start()
    {
        scene_script = scene.GetComponent<Scene_Manager>();
        speed_boost = Resources.Load("Prefabs/Speed_boost");
        InvokeRepeating("spawn", 5f, 7f);

        mine = Resources.Load("Prefabs/mine_not_planted");
        InvokeRepeating("spawn_mine", 7f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(newItem != null)
        {
            Speed_boost speed_boost = newItem.GetComponent<Speed_boost>();
        }
    }

    void spawn()
    {
        if (scene_script.can_move)
        {
            position = new Vector2(Random.Range(x_min, x_max), Random.Range(y_min, y_max));
            newItem = (GameObject)Instantiate(speed_boost,
                                        position,
                                        rotation);
            Destroy(newItem, 12f);
        }
    }

    void spawn_mine()
    {
        if (scene_script.can_move)
        {
            position = new Vector2(Random.Range(x_min, x_max), Random.Range(y_min, y_max));
            newItem = (GameObject)Instantiate(mine,
                                        position,
                                        rotation);
            Destroy(newItem, 12f);
        }
    }
}
