using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSet", menuName = "ObjectSets/RandomSet", order = 1)]
public class RandomObjectSet : ObjectSet 
{
    public enum Mode { RandomOrder, Sequentually }
    private Mode mode = Mode.RandomOrder;

    [SerializeField]
    private GameObject[] gameObjects;
    [SerializeField]
    private WeightedElement[] weights;
    [SerializeField]
    private Randomizer randomizer;

    private static int nextId = 0;

    protected override void Validate()
    {
        if(weights.Length != gameObjects.Length)
        {
            Debug.LogWarning("Length of Weights doesn't match the length of GameObjects!");
        }

        else
        {
            ComputeWeights();
        }
    }

    private void ComputeWeights()
    {
        float totalWeight = 0.0f;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].AccumChance = 0.0f;
            weights[i].Chance = 0.0f;
            totalWeight += weights[i].Weight;
        }


        float earlierChance = 0.0f;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].Chance = weights[i].Weight / totalWeight;
            weights[i].AccumChance = earlierChance + weights[i].Chance;
            earlierChance = weights[i].AccumChance;
        }
    }

    protected override GameObject GetPrefab(ObjectLayer layer, Vector2i id)
    {

        if (mode == Mode.RandomOrder)
        {
            float chance = Random.value;

            for (int i = 0; i < weights.Length; i++)
            {
                if (chance < weights[i].AccumChance)
                    return gameObjects[i];
            }

            Debug.Assert(false);
        }

        else
        {
            GameObject result = gameObjects[nextId];

            nextId = (nextId + 1) % gameObjects.Length;

            return result;
        }

        return null;
    }

    protected override GameObject ModifyInstance(GameObject instance)
    {
        return randomizer.Randomize(instance);
    }

    public override void OnObjectDestroyed(ObjectLayer layer, Vector2i id)
    {
    }

    public override void OnObjectPlaced(ObjectLayer layer, Vector2i id)
    {
    }

    [System.Serializable]
    private class WeightedElement
    {
        [SerializeField]
        private float weight = 1.0f;
        public float Weight { get { return weight; } }
        [SerializeField, ReadOnly]
        private float accumChance = 1.0f;
        public float AccumChance { get { return accumChance; } set { accumChance = value; } }
        [SerializeField, ReadOnly]
        private float chance = 0.0f;
        public float Chance { get { return chance; } set { chance = value; } }
    }

    [System.Serializable]
    public class Randomizer
    {
        public float minScaleX = 1.0f;
        public float maxScaleX = 1.0f;

        public float minScaleY = 1.0f;
        public float maxScaleY = 1.0f;

        public float minRotation = 0.0f;
        public float maxRotation = 0.0f;

        [Range(0.0f, 1.0f)]
        public float chanceMirrorX = 0.0f;
        [Range(0.0f, 1.0f)]
        public float chanceMirrorY = 0.0f;

        public GameObject Randomize(GameObject obj)
        {
            float scaleX = UnityEngine.Random.value * (maxScaleX - minScaleX) + minScaleX;
            float scaleY = UnityEngine.Random.value * (maxScaleY - minScaleY) + minScaleY;
            float rotation = UnityEngine.Random.value * (maxRotation - minRotation) + minRotation;

            float finalScaleX = UnityEngine.Random.value < chanceMirrorX ? -scaleX : scaleX;
            float finalScaleY = UnityEngine.Random.value < chanceMirrorY ? -scaleY : scaleY;

            obj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation);
            obj.transform.localScale = new Vector3(finalScaleX, finalScaleY, obj.transform.localScale.z);
            return obj;
        }

    }
}
