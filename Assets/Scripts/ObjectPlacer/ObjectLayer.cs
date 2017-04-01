using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ObjectLayer : MonoBehaviour 
{
    [System.Flags]
    public enum TileNeighbour
    {
        None = 0,
        Up = 1,
        UpLeft = 2,
        Left = 4,
        Leftdown = 8,
        Down = 16,
        DownRight = 32,
        Right = 64,
        RightUp = 128,
    }

    [SerializeField]
    private ObjectLayerStorage storage;

    [SerializeField]
    private float gridDivision = 1.0f;
    public float GridDivision { get { return gridDivision; } set { gridDivision = value; } }

    void Start()
    {
        if(!storage)
        {
            storage = new GameObject("Storage").AddComponent<ObjectLayerStorage>();
            storage.hideFlags = HideFlags.DontSaveInBuild;
            storage.transform.SetParent(transform);
            storage.RelatedLayer = this;
        }
    }

    public Vector2 GetClosestPointFromIndex(Vector2i index)
    {
        return new Vector2(index.x / gridDivision, index.y / gridDivision);
    }

    public Vector2i CreateIndexFromPos(Vector2 pos, out Vector2 nearestPos)
    {
        pos = RoundToClosestPointOnGrid(pos);
        Vector2i intPos = new Vector2i((int)(pos.x * gridDivision), (int)(pos.y * gridDivision));

        nearestPos = pos;

        return intPos;
    }

    public Vector2 RoundToClosestPointOnGrid(Vector2 pos)
    {
        return VecUtil.RoundToNearestVector2f(new Vector2(pos.x, pos.y), gridDivision);
    }

    public void UpdateNeighbours(ObjectSet set, Vector2i index)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                Vector2i tmpIndex = new Vector2i(index.x + i, index.y + j);
                Vector2 tmpWorldPos = GetClosestPointFromIndex(tmpIndex);

                TryPlaceObjectAt(set, tmpWorldPos, tmpIndex, true, false);
            }
        }
    }

    public GameObject TryPlaceObjectAt(ObjectSet set, Vector2 closestWorldPos, Vector2i index, bool triggersCallback)
    {
        return TryPlaceObjectAt(set, closestWorldPos, index, false, triggersCallback);
    }

    public GameObject TryPlaceObjectAt(ObjectSet set, Vector2 closestWorldPos, Vector2i index, bool onlyReplace, bool triggersCallback)
    {
        if(onlyReplace && !storage.Contains(index))
            return null;
        
        else
            return CreateObjectAt(set, closestWorldPos, index, triggersCallback);
    }

    /// <summary>This methods actually instantiates a GameObject form a prefab, adds it to the internal list at a given position.</summary>
    private GameObject CreateObjectAt(ObjectSet set, Vector2 closestWorldPos, Vector2i index, bool triggersCallback)
    {
        GameObject instance = set.CreateObject(this, index);

        instance.transform.position = closestWorldPos;
        GameObject oldObj = storage.AddObject(instance, index);
        instance.transform.SetParent(transform);

        if (oldObj)
             DestroyImmediate(oldObj);

        if(triggersCallback)
            set.OnObjectPlaced(this, index);

        return instance;
    }

    public void DestroyObjectAt(ObjectSet set, Vector2i intPos)
    {
        GameObject oldObject = storage.RemoveObject(intPos);
        DestroyImmediate(oldObject);
        set.OnObjectDestroyed(this, intPos);
    }

    public void Clear()
    {
        storage.Clear();
    }

    #region BIT_CHECKINGS

    private bool CheckDown(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(0, -1);
        return storage.Contains(neighbourPos);
    }

    private bool CheckUp(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(0, 1);
        return storage.Contains(neighbourPos);
    }

    private bool CheckLeft(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(-1, 0);
        return storage.Contains(neighbourPos);
    }

    private bool CheckRight(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(1, 0);
        return storage.Contains(neighbourPos);
    }

    private bool CheckRightUp(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(1, 1);
        return storage.Contains(neighbourPos);
    }

    private bool CheckUpLeft(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(-1, 1);
        return storage.Contains(neighbourPos);
    }

    private bool CheckLeftDown(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(-1, -1);
        return storage.Contains(neighbourPos);
    }

    private bool CheckDownRight(Vector2i pos)
    {
        Vector2i neighbourPos = pos + new Vector2i(1, -1);
        return storage.Contains(neighbourPos);
    }


    public int GetBitMask4(Vector2i pos)
    {
        TileNeighbour bitMask = TileNeighbour.None;

        if (CheckUp(pos))
            bitMask |= TileNeighbour.Up;

        if (CheckLeft(pos))
            bitMask |= TileNeighbour.Left;

        if (CheckDown(pos))
            bitMask |= TileNeighbour.Down;

        if (CheckRight(pos))
            bitMask |= TileNeighbour.Right;

        return (int)bitMask;
    }

    public int GetBitMask8(Vector2i pos)
    {
        TileNeighbour bitMask = TileNeighbour.None;

        int edgeCount = 0;
        bool hasEdgeUp = false;
        bool hasEdgeLeft = false;
        bool hasEdgeDown = false;
        bool hasEdgeRight = false;

        if (CheckUp(pos))
        {
            bitMask |= TileNeighbour.Up;
            hasEdgeUp = true;
            edgeCount++;
        }

        if (CheckLeft(pos))
        {
            bitMask |= TileNeighbour.Left;
            hasEdgeLeft = true;
            edgeCount++;
        }

        if (CheckDown(pos))
        {
            bitMask |= TileNeighbour.Down;
            hasEdgeDown = true;
            edgeCount++;
        }

        if (CheckRight(pos))
        {
            bitMask |= TileNeighbour.Right;
            hasEdgeRight = true;
            edgeCount++;
        }

        //no adjacent edges, ignore corners OR if one edge is set, ignre corners
        if (edgeCount == 0 || edgeCount == 1)
            return (int)bitMask;

        //if two edges are opposite, the corner doesnt matter
        if (edgeCount == 2)
        {
            if (hasEdgeUp && hasEdgeLeft && CheckUpLeft(pos))
                bitMask |= TileNeighbour.UpLeft;

            if (hasEdgeLeft && hasEdgeDown && CheckLeftDown(pos))
                bitMask |= TileNeighbour.Leftdown;

            if (hasEdgeDown && hasEdgeRight && CheckDownRight(pos))
                bitMask |= TileNeighbour.DownRight;

            if (hasEdgeRight && hasEdgeUp && CheckRightUp(pos))
                bitMask |= TileNeighbour.RightUp;

            return (int)bitMask;
        }

        if (edgeCount == 3)
        {
            if (!hasEdgeDown)
            {
                if (CheckUpLeft(pos))
                    bitMask |= TileNeighbour.UpLeft;

                if (CheckRightUp(pos))
                    bitMask |= TileNeighbour.RightUp;
            }

            else if (!hasEdgeRight)
            {
                if (CheckUpLeft(pos))
                    bitMask |= TileNeighbour.UpLeft;

                if (CheckLeftDown(pos))
                    bitMask |= TileNeighbour.Leftdown;
            }

            else if (!hasEdgeUp)
            {
                if (CheckDownRight(pos))
                {
                    bitMask |= TileNeighbour.DownRight;
                }

                if (CheckLeftDown(pos))
                    bitMask |= TileNeighbour.Leftdown;
            }

            else if (!hasEdgeLeft)
            {
                if (CheckDownRight(pos))
                    bitMask |= TileNeighbour.DownRight;

                if (CheckRightUp(pos))
                    bitMask |= TileNeighbour.RightUp;
            }

            return (int)bitMask;
        }

        //all edges are set: all corners can be relevant:
        if (edgeCount == 4)
        {
            if (CheckUpLeft(pos))
                bitMask |= TileNeighbour.UpLeft;

            if (CheckLeftDown(pos))
                bitMask |= TileNeighbour.Leftdown;

            if (CheckDownRight(pos))
                bitMask |= TileNeighbour.DownRight;

            if (CheckRightUp(pos))
                bitMask |= TileNeighbour.RightUp;

           return (int)bitMask;
        }

        Debug.Log("Case didnt met the cases: " + bitMask);
        Debug.Assert(false);
        return (int)bitMask;
    }

    #endregion


}
