using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : AManager<EventManager>
{
    public class Event : UnityEvent<AGameEvent> { }

    private List<AGameEvent> currentEvents = new List<AGameEvent>();

    private List<AGameEvent> openEvents = new List<AGameEvent>();

    public bool IsAnyEventRunning { get { return currentEvents.Count > 0; } }

    [HideInInspector]
    public Event onEventStart = new Event();
    [HideInInspector]
    public Event onEventEnd = new Event();
    [HideInInspector]
    public Event onEventFailed = new Event();
    [HideInInspector]
    public Event onEventWon = new Event();

    private AGameEvent[] allGameEvents;
    private AGameEvent GetGameEvent(int id)
    {
        if (id < 0 || id >= allGameEvents.Length)
            return null;

        return allGameEvents[id];
    }

    public void StartRandomEvent()
    {
        int eventId = Random.Range(0, openEvents.Count);
        StartEvent(openEvents[eventId]);
    }

    public int AllGameEventsCount { get { return allGameEvents.Length; } }

    protected override void OnAwake()
    {
        allGameEvents = GetComponentsInChildren<AGameEvent>();

        for (int i = 0; i < allGameEvents.Length; i++)
        {
            openEvents.Add(allGameEvents[i]);
        }
    }

    public bool StartEvent(AGameEvent gameEvent)
    {
        if (gameEvent == null || gameEvent.IsRunning)
            return false;

        Debug.Log("StartEvent: " + gameEvent);
        bool result = openEvents.Remove(gameEvent);
        Debug.Assert(result);
        gameEvent.StartEvent();
        onEventStart.Invoke(gameEvent);

        Debug.Assert(!currentEvents.Contains(gameEvent));
        currentEvents.Add(gameEvent);
        return true;
    }

    public void EndEvent(AGameEvent gameEvent)
    {
        Debug.Assert(gameEvent.IsRunning);
        Debug.Assert(currentEvents.Contains(gameEvent));
        Debug.Log("EndEvent: " + gameEvent);

        gameEvent.EndEvent();
        onEventWon.Invoke(gameEvent);

        if (gameEvent.GetGameEventWon())
        {
            gameEvent.OnGameEventWon();
            onEventWon.Invoke(gameEvent);
        }

        else
        {
            onEventFailed.Invoke(gameEvent);
        }


        bool result = currentEvents.Remove(gameEvent);
        Debug.Assert(result);

        Debug.Assert(!openEvents.Contains(gameEvent));
        openEvents.Add(gameEvent);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartEvent(GetGameEvent(0));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartEvent(GetGameEvent(1));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartEvent(GetGameEvent(2));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartEvent(GetGameEvent(3));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartEvent(GetGameEvent(4));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartEvent(GetGameEvent(5));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartEvent(GetGameEvent(6));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartEvent(GetGameEvent(7));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartEvent(GetGameEvent(8));
        }

        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartEvent(GetGameEvent(9));
        }
    }
}
