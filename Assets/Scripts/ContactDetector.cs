using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContactDetector : MonoBehaviour {

    public class Event : UnityEvent<ContactDetector> { }

    [HideInInspector]
    public Event onGrounded = new Event();
    [HideInInspector]
    public Event onUngrounded = new Event();

    private HashSet<Collider2D> collidingObjects = new HashSet<Collider2D>();

    public bool IsGrounded { get { return collidingObjects.Count == 0; } }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Assert(other);
        if (collidingObjects.Contains(other))
            return;
        
        collidingObjects.Add(other);
        onGrounded.Invoke(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Assert(other);
        Debug.Assert(collidingObjects.Contains(other));
        collidingObjects.Remove(other);

        if (!IsGrounded)
            onUngrounded.Invoke(this);
    }

}
