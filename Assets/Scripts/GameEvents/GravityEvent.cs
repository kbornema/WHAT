using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEvent : AGameEvent
{
    private float oldWorldGravity;

    [SerializeField]
    private float worldGravityScale = 0.5f;
    [SerializeField]
    private float minDuration = 5.0f;
    [SerializeField]
    private float maxDuration = 10.0f;

    protected override void _StartEvent()
    {
        oldWorldGravity = GameManager.Instance.WorldGravityScale;
        GameManager.Instance.WorldGravityScale = worldGravityScale;

        float dur = Random.Range(minDuration, maxDuration);

        StartCoroutine(CancelInvoke(dur));
    }

    private IEnumerator CancelInvoke(float dur)
    {
        yield return new WaitForSeconds(dur);
        EventManager.Instance.EndEvent(this);
    }

    protected override void _EndEvent()
    {
        GameManager.Instance.WorldGravityScale = oldWorldGravity;
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
    }
}
