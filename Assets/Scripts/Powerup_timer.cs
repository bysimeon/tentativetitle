using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_timer : MonoBehaviour
{
    public bool start = false;
    public Sprite phase_1;
    public Sprite phase_2;
    public Sprite phase_3;
    public Sprite phase_4;

    private Coroutine routine;
    private Coroutine routine2;

    public float fast_speed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTimer()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().transform.localScale = new Vector3(2, 2, 1);
        startBegin();
    }

    public void startBegin()
    {
        routine = StartCoroutine(begin());
    }

    IEnumerator begin()
    {
        start = true;
        GetComponent<SpriteRenderer>().sprite = phase_1;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1.28f, 1.28f, 1);
        GetComponent<SpriteRenderer>().sprite = phase_2;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = phase_3;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = phase_4;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = false;
        start = false;
    }

    public void call_speed_up()
    {
        if(routine2 != null)
        {
            StopCoroutine(routine2);
        }

        routine2 = StartCoroutine(speed_up());
    }

    IEnumerator speed_up()
    {
        startTimer();
        HookLauncher launcher = transform.parent.GetComponentInChildren<HookLauncher>();
        launcher.LaunchVelocity = fast_speed;
        launcher.travel_speed = fast_speed;
        yield return new WaitForSeconds(4f);
        launcher.LaunchVelocity = 150f;
        launcher.travel_speed = 150f;
    }
}
