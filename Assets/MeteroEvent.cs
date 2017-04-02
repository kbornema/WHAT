using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteroEvent : AGameEvent 
{
    [SerializeField]
    private MeteorRain rain;

    protected override void _StartEvent()
    {
        StartCoroutine(MeteroRoutine());
    }

    private IEnumerator MeteroRoutine()
    {
        rain.StartRain();

        float dur = Random.Range(minDur, maxDur);

        yield return new WaitForSeconds(dur);
            
        rain.EndRain();

        EventManager.Instance.EndEvent(this);
    }

    protected override void _EndEvent()
    {
        
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
        
    }
}
