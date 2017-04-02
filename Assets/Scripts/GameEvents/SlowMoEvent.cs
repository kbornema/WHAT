using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoEvent : AGameEvent 
{
    [SerializeField]
    private float timeScale = 0.5f;

    protected override void _StartEvent()
    {
        float dur = Random.Range(minDur, maxDur);

        Time.timeScale = timeScale;

        StartCoroutine(EndIn(dur));
    }

    private IEnumerator EndIn(float dur)
    {
        yield return new WaitForSeconds(dur);

        EventManager.Instance.EndEvent(this);
    }

    protected override void _EndEvent()
    {
        Time.timeScale = GameManager.Instance.NormalTimeScale;
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
        
    }
}
