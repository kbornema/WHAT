using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurrenderEvent : AGameEvent 
{
    protected override void _StartEvent()
    {
        GameManager.Instance.onEnemyKilledEvent.AddListener(OnEnemyKilled);

        StartCoroutine(SurrenderEventRoutine());
    }

    private IEnumerator SurrenderEventRoutine()
    {
        float dur = Random.Range(minDur, maxDur);

        yield return new WaitForSeconds(dur);

        EventManager.Instance.EndEvent(this);
    }

    private void OnEnemyKilled(Health h, Health.EventInfo i)
    {
        if(i.source && i.source.ThePlayer)
        {
            NormalTurtle turtle = h.RootActor.GetComponent<NormalTurtle>();

            if (turtle && turtle.IsSurrendering)
            {
                GameManager.Instance.AddPlayerPoints(-3 * h.RootActor.PointsOnKill, i.source.ThePlayer);
            }
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
