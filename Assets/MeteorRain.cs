using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRain : MonoBehaviour 
{
    [SerializeField]
    private AProjectile meteroPrefab;

    [SerializeField]
    private GameObject minSpawn;
    [SerializeField]
    private GameObject maxSpawn;

    [SerializeField]
    private float minSpawnTime;
    [SerializeField]
    private float maxSpawnTime;

    [SerializeField]
    private int numSpawnsMin = 1;
    [SerializeField]
    private int numSpawnsMax = 5;

    private bool isRunning = false;

    public void StartRain()
    {
        isRunning = true;
        StartCoroutine(Routine());
    }

    public void EndRain()
    {
        isRunning = false;
    }

    private IEnumerator Routine()
    {
        while(isRunning)
        {
            int spawns = Random.Range(numSpawnsMin, numSpawnsMax + 1);

            for (int i = 0; i < spawns; i++)
            {
                StartCoroutine(SpawnMeteor(Random.value * 0.5f));
            }

            float dur = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(dur);
        }
    }

    private IEnumerator SpawnMeteor(float delay)
    {
        yield return new WaitForSeconds(delay);

        AProjectile instance = Instantiate(meteroPrefab);
        instance.transform.position = Vector3.Lerp(minSpawn.transform.position, maxSpawn.transform.position, Random.value);
        instance.InitProjectile(null, new Vector2(-1, -1).normalized);

        instance.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

        Vector3 scale = instance.transform.localScale;

        scale.x *= Random.Range(0.8f, 1.2f);
        scale.y *= Random.Range(0.8f, 1.2f);
        scale.z *= Random.Range(0.8f, 1.2f);

        instance.transform.localScale = scale;
    }
}


