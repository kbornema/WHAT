using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTurtle : MonoBehaviour 
{
    [SerializeField]
    private ActorDetector actorDetector;

    [SerializeField]
    private JumpDetector jumpDetector;

    [SerializeField]
    private Actor actor;

    [SerializeField]
    private bool playDeathAnim = false;

    [SerializeField]
    private bool harmlessAtDay = true;
    [SerializeField]
    private bool explodeOnCloseness = true;
    [SerializeField]
    private float explodeRadius = 2.0f;
    [SerializeField]
    private AProjectile explosionPrefab;

    [SerializeField]
    private float thinkTimerMin = 1.0f;
    [SerializeField]
    private float thinkTimerMax = 1.0f;
    private bool isThinking = true;

    [SerializeField]
    private float timeBeforeExplosion = 2.0f;

    private Actor targetEnemy;
    private Vector2 toTargetEnemy;

    private float lifeTime = 0.0f;
    

    private void Start()
    {
        StartCoroutine(Think());


            actor.TheHealth.onZeroHealth.AddListener(OnZeroHealth);

        jumpDetector.onTriggerEnter.AddListener(OnJumpTrigger);

        actorDetector.onEnter.AddListener(OnActorEnter);
        actorDetector.onExit.AddListener(OnActorExit);
    }

    private void OnZeroHealth(Health arg0, Health.EventInfo arg1)
    {

        if(playDeathAnim)
        {
            arg0.gameObject.layer = LayerUtil.NoneNumber;

            actor.TheAnimator.TriggerDie();

            StartCoroutine(DestroyIn());
        }

        float rand = Random.value;

        if(rand < GameManager.Instance.GameOptions.GrenadeChance)
        {
            GameObject grenadePickup = Instantiate(GameManager.Instance.GrenadePickupPrefab);
            grenadePickup.gameObject.transform.position = actor.Center.transform.position;
        }

    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(actor.gameObject);
    }

    private void OnActorEnter(ActorDetector arg0, Actor arg1)
    {
        //Debug.Log(arg1.gameObject.name);
        targetEnemy = arg1;
    }

    private void OnActorExit(ActorDetector arg0, Actor arg1)
    {
        if(arg1 == targetEnemy)
            targetEnemy = null;
    }

    private void OnJumpTrigger(JumpDetector arg0, Collider2D arg1)
    {
        actor.Jump();
    }

    private IEnumerator Think()
    {
        isThinking = true;

        while(isThinking)
        {
            float time = Random.Range(thinkTimerMin, thinkTimerMax);
            yield return new WaitForSeconds(time);

            DecideDirection();
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;

        if(targetEnemy && (EventManager.Instance.IsDark || !harmlessAtDay))
        {
            if (targetEnemy.TheHealth.CurHitpoints == 0)
            {
                DecideDirection();
                targetEnemy = null;
                return;
            }

            toTargetEnemy = targetEnemy.Center.transform.position - actor.Center.transform.position;

            if (explodeOnCloseness && lifeTime > timeBeforeExplosion)
            {
                float dist = toTargetEnemy.magnitude;

                if (dist < explodeRadius)
                {
                    actor.TheHealth.ApplyHealth(new Health.EventInfo(-actor.TheHealth.MaxHitpoints, null));

                    AProjectile explosionInstance = Instantiate(explosionPrefab);
                    explosionInstance.InitProjectile(actor, Vector2.zero);
                    explosionInstance.transform.position = actor.Center.transform.position;
                    return;
                }
            }
           
            toTargetEnemy.Normalize();

            if(toTargetEnemy.y > actor.Center.transform.position.y)
            {
                actor.Jump();
            }

            actor.SetMoveX(toTargetEnemy.x);
        }
    }

    private void DecideDirection()
    {
        float rand = Random.value;

        if(rand < 0.33)
        {
            actor.MoveDirection = new Vector2(1.0f, 0.0f);
        }

        else if(rand < 0.66)
        {
            actor.MoveDirection = new Vector2(-1.0f, 0.0f);
        }

        else
        {
            actor.MoveDirection = new Vector2(0.0f, 0.0f);
        }
    }



}
