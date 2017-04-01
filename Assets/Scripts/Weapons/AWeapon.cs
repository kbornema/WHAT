using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    public enum FireMode { Click, Press, Release }

    [SerializeField]
    protected FireMode mode;
    public FireMode Mode { get { return mode; } }

    [SerializeField]
    protected GameObject spawnPos;

    public abstract bool TryShoot(Actor source, Vector2 dir);
}

