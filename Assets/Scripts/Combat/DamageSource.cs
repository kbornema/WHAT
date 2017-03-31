using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageSource : MonoBehaviour
{
    public class Event : UnityEngine.Events.UnityEvent<DamageSource, Collider2D> { }
    public enum DamageMode { OnEnter, OnStay, OnExit }
     
    [SerializeField] 
    private DamageMode _mode = DamageMode.OnEnter;
    [SerializeField] 
    private float _hitCooldown = 0.5f;
    [SerializeField] 
    private Damage _damage;
    [SerializeField]
    private Actor _source;
    public Actor Source { get { return _source; } set { _source = value; } }

    [HideInInspector] 
    public Event onDamageDealt = new Event();
    
    private Dictionary<Collider2D, Entry> _collidingObjects = new Dictionary<Collider2D, Entry>();
    private bool _collidingObjectsDirty = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_mode == DamageMode.OnEnter)
        {
            ApplyDamage(other, other.GetComponent<IDamageable>());
        }

        else
        {
            //sometimes the collision detection is buggy (Unity-wise) then several on enters might occur without an exit earlier:
            if (_collidingObjects.ContainsKey(other))
                return;

            IDamageable damagable = other.GetComponent<IDamageable>();

            if(damagable != null)
            {
                _collidingObjects.Add(other, new Entry(other, damagable, _hitCooldown));
            }
        }
    }

    private void Update()
    {
        if (_mode == DamageMode.OnStay)
        {
            foreach(Entry e in _collidingObjects.Values)
            {
                if(e.collider == null)
                {
                    _collidingObjectsDirty = true;
                    continue;
                }

                e.hitCooldown -= Time.deltaTime;

                if (e.hitCooldown <= 0.0f)
                {
                    ApplyDamage(e.collider, e.damageable);
                    e.ResetCooldown();
                }
            }


            if (_collidingObjectsDirty)
            {
                RebuildDictionary();
            }
        }

    }

    //when an Actor dies within onTriggerStay then its collider will be deleted -> the dictionary has a null-key. This methods rebuilds the dictionary and removes null entries:
    private void RebuildDictionary()
    {
        Dictionary<Collider2D, Entry> newDict = new Dictionary<Collider2D, Entry>();

        foreach(Entry e in _collidingObjects.Values)
        {
            if(e.collider != null)
            {
                newDict.Add(e.collider, e);
            }
        }

        _collidingObjects = newDict;
        _collidingObjectsDirty = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_mode == DamageMode.OnEnter)
            return;

        else
        {
            if(_collidingObjects.ContainsKey(other))
            {
                if (_mode == DamageMode.OnExit)
                    ApplyDamage(other, _collidingObjects[other].damageable);

                _collidingObjects.Remove(other);
            }
        }
    }

    private void ApplyDamage(Collider2D collider, IDamageable damagable)
    {
        if (collider == null)
            return;

        //only deal damage if the tag is different from this damageSource
        if(damagable != null && !gameObject.CompareTag(collider.tag))
        {
            damagable.ApplyDamage(_source, _damage);
            onDamageDealt.Invoke(this, collider);
        }
    }

    private class Entry
    {
        public float hitCooldown;
        public float maxHitCooldown;
        public Collider2D collider;
        public IDamageable damageable;
        
        public void ResetCooldown()
        {
            this.hitCooldown = this.maxHitCooldown;
        }

        public Entry(Collider2D collider, IDamageable damageable, float cooldown)
        {
            this.collider = collider;
            this.maxHitCooldown = cooldown;
            this.damageable = damageable;
            this.hitCooldown = 0.0f;
        }
    }
}
