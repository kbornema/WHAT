using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
    [SerializeField]
    private Vector2 startDirection;
    [SerializeField]
    private GameObject spawnPos;

    public Actor Spawn(Actor prefab)
    {
        Actor instance = Instantiate(prefab);
        instance.transform.position = spawnPos.transform.position;

        return instance;
    }

    private void Start()
    {
        GameManager.Instance.RegisterEnemySpawner(this);
    }
}
