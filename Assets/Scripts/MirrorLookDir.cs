using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLookDir : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GetComponent<Actor>().onLookDirChanged.AddListener(OnLookDirChanged);	
	}

    private void OnLookDirChanged(Actor actor)
    {
        Vector3 scale = gameObject.transform.localScale;

        if (actor.LookDirection.x < 0.0f)
        {
            scale.x = -Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }

        else if (actor.LookDirection.x > 0.0f)
        {
            scale.x = Mathf.Abs(scale.x);
            gameObject.transform.localScale = scale;
        }
    }
	
}
