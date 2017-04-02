using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour {

	// Use this for initialization
	void Awake () 
    {
        gameObject.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        
        Vector3 scale = gameObject.transform.localScale;

        scale.x *= Random.Range(0.75f, 1.25f);
        scale.y *= Random.Range(0.75f, 1.25f);
        scale.z *= Random.Range(0.75f, 1.25f);

        if (Random.value < 0.5f)
            scale.x = -scale.x;

        if (Random.value < 0.5f)
            scale.y = -scale.y;

        gameObject.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    	//TODO: fade:
	}
}
