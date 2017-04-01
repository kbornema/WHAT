using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiledSet", menuName = "ObjectSets/TiledSet", order = 0)]
public class TiledObjectSet : ObjectSet 
{
    public enum TileMode { Bit4Tiled = 16, Bit8Tiled = 47 }

    [SerializeField]
    private bool updateNeighbours = true;

    [SerializeField]
    private TileMode mode = TileMode.Bit4Tiled;
    [SerializeField]
    private GameObject[] objects;

    protected override void Validate()
    {
        if (objects.Length != (int)mode)
        {
            Debug.LogWarning("Provided GameObjects don't match the expected number!");
        }
    }

    protected override GameObject GetPrefab(ObjectLayer layer, Vector2i id)
    {
        int prefabId = GetPrefabId(layer, id);

        Debug.Assert(prefabId != -1);
        return objects[prefabId];
    }

    protected override GameObject ModifyInstance(GameObject instance)
    {
        return instance;
    }

    private int GetPrefabId(ObjectLayer layer, Vector2i id)
    {
        if (mode == TileMode.Bit8Tiled)
        {
            int bitMask = layer.GetBitMask8(id);

            for (int i = 0; i < ObjectPlacerConstants.TILE_ID_TO_BITS_8.Length; i++)
            {
                if (ObjectPlacerConstants.TILE_ID_TO_BITS_8[i] == bitMask)
                    return i;
            }
        }

        else if (mode == TileMode.Bit4Tiled)
        {
            int bitMask = layer.GetBitMask4(id);

            for (int i = 0; i < ObjectPlacerConstants.TILE_ID_TO_BITS_4.Length; i++)
            {
                if (ObjectPlacerConstants.TILE_ID_TO_BITS_4[i] == bitMask)
                    return i;
            }
        }

        return -1;
        
    }

    public override void OnObjectDestroyed(ObjectLayer layer, Vector2i id)
    {
        if (updateNeighbours)
            layer.UpdateNeighbours(this, id);
    }

    public override void OnObjectPlaced(ObjectLayer layer, Vector2i id)
    {
        if (updateNeighbours)
            layer.UpdateNeighbours(this, id);
    }
}
