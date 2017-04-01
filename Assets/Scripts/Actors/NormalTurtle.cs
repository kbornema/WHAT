using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTurtle : MonoBehaviour 
{
    [SerializeField]
    private JumpDetector jumpDetector;

    [SerializeField]
    private Actor actor;

    [SerializeField]
    private float thinkTimerMin = 1.0f;
    [SerializeField]
    private float thinkTimerMax = 1.0f;
    private bool isThinking = true;

    private void Start()
    {
        StartCoroutine(Think());

        jumpDetector.onTriggerEnter.AddListener(OnJumpTrigger);
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