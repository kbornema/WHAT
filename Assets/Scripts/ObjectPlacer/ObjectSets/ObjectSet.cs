using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ObjectSet : ScriptableObject 
{
    protected abstract void Validate();

    protected abstract GameObject GetPrefab(ObjectLayer layer, Vector2i id);

    protected abstract GameObject ModifyInstance(GameObject instance);

    public abstract void OnObjectDestroyed(ObjectLayer layer, Vector2i id);

    public abstract void OnObjectPlaced(ObjectLayer layer, Vector2i id);

    public GameObject CreateObject(ObjectLayer layer, Vector2i id)
    {
        return ModifyInstance(InstantiateFromPrefab(GetPrefab(layer, id)));
    }

    private void OnValidate()
    {
        Validate();
    }

    private GameObject InstantiateFromPrefab(GameObject prefab)
    {
        return (GameObject)PrefabUtility.InstantiatePrefab(prefab);
    }
}
