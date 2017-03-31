using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour 
{
    [SerializeField]
    private Actor actor;
    public Actor TheActor { get { return actor; } set { actor = value; } }

    [SerializeField]
    private bool rotateWithLookDir;

    private float minOffset;
    private float maxOffset;

	
	// Update is called once per frame
	void LateUpdate () 
    {
        gameObject.transform.position = actor.Center.transform.pos2() + actor.LookDirection;

        if(rotateWithLookDir)
        {
            gameObject.transform.up = actor.LookDirection;
        }
	}
}
