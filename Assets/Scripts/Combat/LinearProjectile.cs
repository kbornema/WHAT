using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : AProjectile 
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _speedDeviation = 0.0f;

    private float finalSpeed;

    private Vector2 _direction;
    
    protected override void InitProjectile(Vector2 dir)
    {
        this._direction = dir;
        this.finalSpeed = _speed + _speedDeviation * (Random.value - 0.5f);
    }

    private void FixedUpdate()
    {
        if(_rigidbody)
            _rigidbody.position += _direction * finalSpeed * Time.fixedDeltaTime;
    }   
    
}
