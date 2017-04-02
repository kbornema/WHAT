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
    private bool harmlessAtDay = true;


    [SerializeField]
    private float thinkTimerMin = 1.0f;
    [SerializeField]
    private float thinkTimerMax = 1.0f;
    private bool isThinking = true;

    private Actor targetEnemy;
    private Vector2 toTargetEnemy;

    private void Start()
    {
        StartCoroutine(Think());

        jumpDetector.onTriggerEnter.AddListener(OnJumpTrigger);

        actorDetector.onEnter.AddListener(OnActorEnter);
        actorDetector.onExit.AddListener(OnActorExit);
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
        if(targetEnemy && (EventManager.Instance.IsDark || !harmlessAtDay))
        {
            if (targetEnemy.TheHealth.CurHitpoints == 0)
            {
                DecideDirection();
                targetEnemy = null;
                return;
            }

            toTargetEnemy = targetEnemy.Center.transform.position - actor.Center.transform.position;
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
