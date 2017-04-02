using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAcid : MonoBehaviour {

    AudioSource source;
    public AudioClip acidLoop;
    bool looping;

    // Use this for initialization
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        looping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying == false)
        {
            if (looping == false)
            {
                source.clip = acidLoop;
                source.loop = true;
                source.Play();
                looping = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
