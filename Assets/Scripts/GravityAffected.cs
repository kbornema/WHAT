using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffected : MonoBehaviour 
{
    [SerializeField]
    private float myGravityScale = 1.0f;
    [SerializeField]
    private Rigidbody2D _rigidbody;

    private void Reset()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        GameManager.Instance.onGravityChanged.AddListener(OnGravityChanged);
        OnGravityChanged(GameManager.Instance);
    }

    private void OnGravityChanged(GameManager gm)
    {
        _rigidbody.gravityScale = gm.WorldGravityScale * myGravityScale;
    }

}
