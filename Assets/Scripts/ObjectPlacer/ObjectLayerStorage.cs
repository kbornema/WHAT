using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class that stores references to GameObjects that have been placed onto an ObjectLayer. </summary>
public class ObjectLayerStorage : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private ObjectLayer relatedLayer;
    public ObjectLayer RelatedLayer { get { return relatedLayer; } set { relatedLayer = value; } }

    private Dictionary<Vector2i, GameObject> objectSet = new Dictionary<Vector2i, GameObject>();
    [SerializeField]
    private List<StorageElementSerialization> serializationList = new List<StorageElementSerialization>();

    public GameObject GetObject(int x, int y)
    {
        return GetObject(new Vector2i(x, y));
    }

    public GameObject GetObject(Vector2i id)
    {
        return objectSet[id];
    }

    public GameObject AddObject(GameObject obj, int x, int y)
    {
        return AddObject(obj, new Vector2i(x, y));
    }

    public GameObject AddObject(GameObject obj, Vector2i id)
    {
        GameObject oldObject = null;

        if(objectSet.TryGetValue(id, out oldObject))
        {
            objectSet[id] = obj;
        }
        else
        {
            objectSet.Add(id, obj);
        }

        return oldObject;
    }

    public GameObject RemoveObject(Vector2i id)
    {
        GameObject objectAtId = null;

        if(objectSet.TryGetValue(id, out objectAtId))
            objectSet.Remove(id);

        return objectAtId;

    }

    public bool Contains(Vector2i key)
    {
        return objectSet.ContainsKey(key);
    }

    public void OnAfterDeserialize()
    {
        if (objectSet == null)
            objectSet = new Dictionary<Vector2i, GameObject>();

        this.objectSet.Clear();

        for (int i = 0; i < serializationList.Count; i++)
            this.objectSet.Add(serializationList[i].index, serializationList[i].gameObject);
    }

    public void OnBeforeSerialize()
    {
        if (this.serializationList == null)
            this.serializationList = new List<StorageElementSerialization>();

        this.serializationList.Clear();

        foreach (KeyValuePair<Vector2i, GameObject> pair in this.objectSet)
            this.serializationList.Add(new StorageElementSerialization(pair.Key, pair.Value));
    }

    public void Clear()
    {
        for (int i = 0; i < serializationList.Count; i++)
        {
            DestroyImmediate(serializationList[i].gameObject);
        }

        objectSet.Clear();
        serializationList.Clear();
    }

    [System.Serializable]
    public class StorageElementSerialization
    {
        public Vector2i index;
        public GameObject gameObject;

        public StorageElementSerialization(Vector2i index, GameObject gameObject)
        {
            this.index = index;
            this.gameObject = gameObject;
        }
    }
}
