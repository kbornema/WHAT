using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour 
{
    public enum DestroyMode { None, OnSameTag, OnDifferentTag }

    [SerializeField]
    protected DamageSource _damageSource;
    [SerializeField]
    protected Rigidbody2D _rigidbody;
    [SerializeField]
    protected DestroyMode _destroyMode;
    [SerializeField]
    protected bool _inheritTagFromSource = false;
    [SerializeField]
    protected LayerMask _destroyLayers;
    [SerializeField]
    protected float _lifeTime;
    [SerializeField, ReadOnly]
    protected float _curLifeTime;

    [SerializeField]
    private bool rotateWithDirection = true;

    protected Actor _sourceActor;
    public Actor SourceActor { get { return _sourceActor; } }

    protected virtual void Reset()
    {
        if (_damageSource == null)
            _damageSource = GetComponent<DamageSource>();

        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if(_curLifeTime > 0.0f)
        {
            _curLifeTime -= Time.deltaTime;

            if(_curLifeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_destroyMode == DestroyMode.None)
            return;

        //check if the layer of the colliding object is part of "_destroyLayers"
        if ((_destroyLayers & (1 << other.gameObject.layer)) != 0)
        {
            bool isSameTag = other.tag == gameObject.tag;

            if (_destroyMode == DestroyMode.OnSameTag && isSameTag)
            {
                Destroy(gameObject);
            }

            else if (_destroyMode == DestroyMode.OnDifferentTag && !isSameTag)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void InitProjectile(Actor sourceActor, Vector2 direction)
    {
        this._sourceActor = sourceActor;

        if(this._damageSource)
            this._damageSource.Source = this._sourceActor;

        if (_inheritTagFromSource && this._sourceActor)
        {
            gameObject.tag = this._sourceActor.gameObject.tag;

            if(this._damageSource && gameObject != _damageSource.gameObject)
                _damageSource.tag = this._sourceActor.gameObject.tag;
        }

        _curLifeTime = _lifeTime;

        if (rotateWithDirection)
        {
            gameObject.transform.up = direction;
        }

        InitProjectile(direction);
    }

    protected abstract void InitProjectile(Vector2 dir);
}
