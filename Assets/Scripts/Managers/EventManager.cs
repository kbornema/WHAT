using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : AManager<EventManager>
{
    public class Event : UnityEvent<AGameEvent> { }
    
    private AGameEvent currentEvent;
    public AGameEvent CurrentEvent { get {return currentEvent; } }

    public bool IsEventRunning { get { return currentEvent != null; } }

    [HideInInspector]
    public Event onEventStart = new Event();
    [HideInInspector]
    public Event onEventEnd = new Event();
    [HideInInspector]
    public Event onEventFailed = new Event();
    [HideInInspector]
    public Event onEventWon = new Event();

    protected override void OnAwake()
    {
       
    }

    public void StartEvent(AGameEvent ev)
    {
        Debug.Assert(ev != null);

        if (currentEvent == null)
        {
            currentEvent = ev;
            currentEvent.StartEvent();
            onEventStart.Invoke(ev);
        }
    }

    public void EndEvent()
    {
        if(currentEvent != null)
        {
            currentEvent.EndEvent();
            onEventWon.Invoke(currentEvent);

            if (currentEvent.GetGameEventWon())
            {
                currentEvent.OnGameEventWon();
                onEventWon.Invoke(currentEvent);
            }

            else
            {
                onEventFailed.Invoke(currentEvent);
            }

            currentEvent = null;
        }
    }

            

    
}
