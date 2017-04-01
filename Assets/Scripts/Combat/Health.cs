using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public class Event : UnityEvent<Health, EventInfo> { }

    [HideInInspector] 
    public Event onHealthChanged = new Event();
    [HideInInspector] 
    public Event onZeroHealth = new Event();

    [SerializeField] 
    private GameObject _rootObject;
    public GameObject RootObject { get { return _rootObject; } }

    [SerializeField]
    private Actor _rootActor;
    public Actor RootActor { get { return _rootActor; } }

    [SerializeField] 
    private float _maxHitpoints;
    [SerializeField, ReadOnly] 
    private float _curHitpoints;
    
    public int CurHitpoints { get { return (int)_curHitpoints; } }
    public int MaxHitpoints { get { return (int)_maxHitpoints; } }

    public float HitpointPercent { get { return Mathf.Clamp(_curHitpoints / _maxHitpoints, 0.0f, 1.0f); } }
    
    private void Reset()
    {
        if (_rootObject == null)
            _rootObject = gameObject;

        HealToMaxHitpoints();
    }

    private void OnValidate()
    {
        Reset();
    }

    private void Awake()
    {
        Reset();
    }

    private void HealToMaxHitpoints()
    {
        this._curHitpoints = this._maxHitpoints;
        onHealthChanged.Invoke(this, new EventInfo());
    }

    private void ChangeHealth(EventInfo info)
    {
        this._curHitpoints += info.delta;

        onHealthChanged.Invoke(this, info);

        if (this._curHitpoints > this._maxHitpoints)
        {
            this._curHitpoints = this._maxHitpoints;
        }

        else if(this._curHitpoints <= 0)
        {
            this._curHitpoints = 0;
            onZeroHealth.Invoke(this, info);
        }
    }

    public void ApplyHealth(EventInfo info)
    {
        ChangeHealth(info);
    }

    public void Refill()
    {
        ApplyHealth(new EventInfo(this.MaxHitpoints, null));
    }

    public class EventInfo
    {
        public float delta;
        public Actor source;

        public EventInfo()
        {
        }

        public EventInfo(float delta)
        {
            this.delta = delta;
        }

        public EventInfo(float delta, Actor source)
            : this(delta)
        {
            this.source = source;
        }
    }
}
