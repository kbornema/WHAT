using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSingleRandomPitch : MonoBehaviour {

    AudioSource source;

    public float minPitch = 1;
    public float maxPitch = 1;

    // Use this for initialization
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.pitch = Random.Range(minPitch, maxPitch);
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
    }
}
