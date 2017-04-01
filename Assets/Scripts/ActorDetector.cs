using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorDetector : MonoBehaviour 
{
    public class Event : UnityEvent<ActorDetector, Actor> { }

    [HideInInspector]
    public Event onEnter = new Event();
    [HideInInspector]
    public Event onExit = new Event();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag != gameObject.tag)
        {
            Actor actor = other.GetComponent<Actor>();

            if(actor)
            {
                onEnter.Invoke(this, actor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != gameObject.tag)
        {
            Actor actor = other.GetComponent<Actor>();

            if (actor)
            {
                onExit.Invoke(this, actor);
            }
        }
    }
}
