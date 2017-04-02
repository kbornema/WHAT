using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderEvent : AGameEvent 
{
    protected override void _StartEvent()
    {
        GameManager.Instance.onEnemyKilledEvent.AddListener(OnEnemyKilled);
    }

    private void OnEnemyKilled(Health h, Health.EventInfo i)
    {
        if(i.source && i.source.ThePlayer)
        {
            GameManager.Instance.AddPlayerPoints(h.RootActor.PointsOnKill * -2, i.source.ThePlayer);
        }
    }

    protected override void _EndEvent()
    {
        GameManager.Instance.onEnemyKilledEvent.RemoveListener(OnEnemyKilled);
    }

    public override bool GetGameEventWon()
    {
        return true;
    }

    public override void OnGameEventWon()
    {
        
    }
}
