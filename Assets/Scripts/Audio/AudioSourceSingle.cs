﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSingle : MonoBehaviour {

    AudioSource source;

	// Use this for initialization
	void Start () {
        source = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (source.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
	}
}
