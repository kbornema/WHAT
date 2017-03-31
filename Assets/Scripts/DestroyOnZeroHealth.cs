using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnZeroHealth : MonoBehaviour 
{
	private void Start () 
    {
        GetComponent<Health>().onZeroHealth.AddListener(OnZeroHealth);
	}

    private void OnZeroHealth(Health h, Health.EventInfo info)
    {
        Destroy(h.RootObject);
    }
}
