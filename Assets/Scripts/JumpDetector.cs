using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpDetector : MonoBehaviour 
{
    public class Event : UnityEvent<JumpDetector, Collider2D> { }

    [HideInInspector]
    public Event onTriggerEnter = new Event();

    public void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter.Invoke(this, other);
    }
}
