using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AGameEvent : MonoBehaviour 
{
    [SerializeField]
    protected int pointsOnWin = 0;
    public int PointsOnWin { get { return pointsOnWin; } }

    [SerializeField, ReadOnly]
    protected bool isRunning = false;
    public bool IsRunning { get { return isRunning; } }

    public void StartEvent()
    {
        isRunning = true;
        _StartEvent();
    }

    protected abstract void _StartEvent();

    public void EndEvent()
    {
        isRunning = false;
        _EndEvent();
    }
    
    protected abstract void _EndEvent();
    
    public abstract bool GetGameEventWon();

    public abstract void OnGameEventWon();
}
