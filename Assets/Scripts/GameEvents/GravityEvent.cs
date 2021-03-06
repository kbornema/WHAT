using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEvent : AGameEvent
{
    private float oldWorldGravity;

    [SerializeField]
    private float worldGravityScale = 0.5f;

    protected override void _StartEvent()
    {
        oldWorldGravity = GameManager.Instance.WorldGravityScale;
        GameManager.Instance.WorldGravityScale = worldGravityScale;
        SoundManager.Instance.StartSingleSound(SoundManager.Sound.Gravity);
        float dur = Random.Range(minDur, maxDur);

        StartCoroutine(CancelInvoke(dur));
    }

    private IEnumerator CancelInvoke(float dur)
    {
        EventManager.Instance.WindParticles.Play();
        yield return new WaitForSeconds(dur);
        EventManager.Instance.WindParticles.Stop();
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
