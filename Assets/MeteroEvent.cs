using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteroEvent : AGameEvent 
{
    [SerializeField]
    private MeteorRain rain;

    protected override void _StartEvent()
    {
       
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
