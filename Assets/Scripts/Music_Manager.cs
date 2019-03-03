using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip music;

    private static Music_Manager instance;

    public AudioClip stage1_music;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        source = GetComponent<AudioSource>();
        if (!source.isPlaying)
        {
            source.PlayOneShot(music, .5f);
        }
    }

    public void StopMusic()
    {
        source.Stop();
    }

    public void PlayStage1Music()
    {
        source = GetComponent<AudioSource>();
        if (!source.isPlaying)
        {
            source.PlayOneShot(stage1_music, .3f);
        }
    }
}
