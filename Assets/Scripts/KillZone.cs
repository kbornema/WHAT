using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if(player)
        {
            Debug.Log("PlayerTouch");
        }

        else
        {
            Debug.Log(other.gameObject.name);
            Destroy(other.gameObject);
        }
    }
}
