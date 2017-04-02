using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : AProjectile 
{
    [SerializeField]
    private float force = 5.0f;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private AProjectile explosionPrefab;

    protected override void InitProjectile(Vector2 dir)
    {
        this._rigidbody.AddForce(dir * force, ForceMode2D.Impulse);
        StartCoroutine(GranadeRoutine());
    }

    private IEnumerator GranadeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);

        AProjectile explosion = Instantiate(explosionPrefab);

        SoundManager.Instance.StartSingleSound(SoundManager.Sound.Explosion);

        explosion.InitProjectile(this._sourceActor, Vector2.zero);

        explosion.transform.position = gameObject.transform.position;

        Destroy(gameObject);
    }


}
