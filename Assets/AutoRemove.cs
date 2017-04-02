using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemove : MonoBehaviour 
{
    [SerializeField]
    private Actor actor;

    [SerializeField]
    private float lifeTime = 8.0f;
	
	// Update is called once per frame
	void Update () 
    {
        if(actor && actor.TheHealth.CurHitpoints > 0)
        {
            lifeTime -= Time.deltaTime;

            if(lifeTime <= 0.0f)
            {
                actor.TheHealth.ApplyHealth(new Health.EventInfo(-actor.TheHealth.MaxHitpoints, actor));
            }
        }
	}
}
