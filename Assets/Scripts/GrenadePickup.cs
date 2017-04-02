using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickup : MonoBehaviour 
{
	private void OnTriggerEnter2D(Collider2D other)
    {
        Actor actor = other.GetComponent<Actor>();

        if(actor && actor.ThePlayer)
        {

            if(actor.ThePlayer.WeaponRight)
            {
                SimpleProjectileWeapon we = (actor.ThePlayer.WeaponRight as SimpleProjectileWeapon);

                if(we)
                    we.AddAmmo(2);
            }
            
            Destroy(gameObject);
        }
    }
}
