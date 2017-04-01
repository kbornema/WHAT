using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other)
    {
        ContactDetector detector = other.gameObject.GetComponent<ContactDetector>();

        if (detector)
            return;

        Health health = other.gameObject.GetComponent<Health>();

        if(health && health.RootActor && health.RootActor.ThePlayer)
        {
            health.RootActor.ThePlayer.Kill();
        }

        else
        {
            Destroy(other.gameObject);
        }
    }
}
