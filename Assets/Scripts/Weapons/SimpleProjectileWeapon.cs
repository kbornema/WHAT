using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileWeapon : AWeapon
{
    [SerializeField]
    protected float knockback = 0.0f;

    [SerializeField]
    protected float cooldown = 0.0f;

    [SerializeField]
    protected float spreadDegree = 25.0f;

    [SerializeField]
    protected AProjectile projectilePrefab;

    [SerializeField]
    protected int curAmmo = 0;
    public int CurAmmo{ get { return curAmmo; } }
    [SerializeField]
    protected int maxAmmo = 0;
    [SerializeField]
    protected bool usesAmmo = false;

    [SerializeField]
    protected int shotsPerTrigger = 1;
    
    protected bool canBeShot = true;
    public bool CanBeShot { get { return canBeShot; } }

    public void AddAmmo(int amount)
    {
        this.curAmmo = Mathf.Clamp(this.curAmmo + amount, 0, this.maxAmmo + 1);
    }

    public override bool TryShoot(Actor source, Vector2 dir)
    {
        if (usesAmmo && curAmmo <= 0)
        {
            return false;
        }

        if(canBeShot)
        {
            for (int i = 0; i < shotsPerTrigger; i++)
            {
                AProjectile instance = Instantiate(projectilePrefab);

                instance.gameObject.transform.position = spawnPos.transform.position;

                instance.InitProjectile(source, dir.Rotate((Random.value - 0.5f) * Mathf.Deg2Rad * spreadDegree));
            }

            if(shotsPerTrigger > 1)
                SoundManager.Instance.StartSingleSound(SoundManager.Sound.Shotgun, 1.1f);
            else
                SoundManager.Instance.StartSingleSoundRandomPitch(SoundManager.Sound.Bullet, 0.8f);


            source.AddForce(-dir * knockback, ForceMode2D.Impulse);

            StartCoroutine(WaitForReady());

            if (usesAmmo)
                curAmmo--;

            
            return true;
        }

        return false;
    }

    private IEnumerator WaitForReady()
    {
        canBeShot = false;

        if(cooldown > 0.0f)
            yield return new WaitForSeconds(cooldown);

        canBeShot = true;
    }

}
