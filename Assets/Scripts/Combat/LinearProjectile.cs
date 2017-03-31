using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : AProjectile 
{
    [SerializeField]
    private float _speed;

    private Vector2 _direction;
    
    protected override void InitProjectile(Vector2 dir)
    {
        this._direction = dir;
    }

    private void FixedUpdate()
    {
        _rigidbody.position += _direction * _speed * Time.fixedDeltaTime;
    }
    
}
