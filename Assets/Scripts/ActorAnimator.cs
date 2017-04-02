using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimator : MonoBehaviour 
{
    private const string JUMP_TRIGGER = "jump";
    private const string DIE_TRIGGER = "die";
    private const string IS_MOVING_BOOL = "isMoving";
    private const string IS_GROUNDED_BOOL = "isGrounded";

    private const string JUMP_SPEED = "jumpSpeed";
    private const string FALL_SPEED = "fallSpeed";
    private const string IDLE_SPEED = "idleSpeed";
    private const string RUN_SPEED = "runSpeed";
    private const string DIE_SPEED = "dieSpeed";

    [SerializeField]
    private Animator animator;
    public Animator TheAnimator { get { return animator; } }

    [SerializeField]
    private float jumpTime = 1.0f;
    [SerializeField]
    private float dieTime = 1.0f;
    [SerializeField]
    private float runTime = 1.0f;
    [SerializeField]
    private float idleTime = 1.0f;
    [SerializeField]
    private float fallTime = 1.0f;

    private void Start()
    {
        SetIdleTime(idleTime);
        SetJumpTime(jumpTime);
        SetDieTime(dieTime);
        SetRunTime(runTime);
        SetFallTime(fallTime);
    }

    private void SetFallTime(float fallTime)
    {
        this.fallTime = fallTime;
        animator.SetFloat(FALL_SPEED, 1.0f / fallTime);
    }

    private void SetRunTime(float runTime)
    {
        this.runTime = runTime;
        animator.SetFloat(RUN_SPEED, 1.0f / runTime);
    }

    private void SetDieTime(float dieTime)
    {
        this.dieTime = dieTime;
        animator.SetFloat(DIE_SPEED, 1.0f / dieTime);
    }

    private void SetJumpTime(float jumpTime)
    {
        this.jumpTime = jumpTime;
        animator.SetFloat(JUMP_SPEED, 1.0f / jumpTime);
    }

    private void SetIdleTime(float idleTime)
    {
        this.idleTime = idleTime;
        animator.SetFloat(IDLE_SPEED, 1.0f / idleTime);
    }

    public void TriggerJump()
    {
        animator.SetTrigger(JUMP_TRIGGER);
    }

    public void TriggerDie()
    {
        animator.SetTrigger(DIE_TRIGGER);
    }

    public void SetMove(bool val)
    {
        animator.SetBool(IS_MOVING_BOOL, val);
    }

    public void SetGrounded(bool val)
    {
        animator.SetBool(IS_GROUNDED_BOOL, val);
    }
    
    public void TriggerRespawn()
    {
        animator.SetTrigger("respawn");
    }
}
