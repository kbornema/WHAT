using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPlacerWindow : EditorWindow 
{
    private enum ToolMode { Single, Rectangle }

    private static ObjectPlacerWindow window;

    private static ObjectLayer selectedLayer;
    private static ObjectSet placableObjects;

    private static ToolMode toolMode = ToolMode.Single;

    private static Vector2 rectangleStart;

    private static Vector2 minRectPos;
    private static Vector2 maxRectPos;
    private static bool drawRect;

    [MenuItem("Tool/ObjectPlacerWindow _F1")]
    public static void OpenWindow()
    {
        window = EditorWindow.GetWindow<ObjectPlacerWindow>();

        if(Selection.activeGameObject)
        {
            ObjectLayer potentialRoot = Selection.activeGameObject.GetComponent<ObjectLayer>();

            if (potentialRoot != null)
                ObjectPlacerWindow.selectedLayer = potentialRoot;

            else
            {
                ObjectLayerStorage storage = Selection.activeGameObject.GetComponent<ObjectLayerStorage>();

                if (storage)
                    ObjectPlacerWindow.selectedLayer = storage.RelatedLayer;
            }
        }

        Tools.current = Tool.View;
    }
	
	private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneUpdate;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneUpdate;
    }

    private void OnSceneUpdate(SceneView sceneView)
    {
        if (Event.current.isMouse)
            ProcessMouseEvents(sceneView);

        else if (Event.current.isKey)
            ProcessKeyEvents();

        if(drawRect)
            DrawWireRectangle(minRectPos, maxRectPos);
    }

    private void ProcessKeyEvents()
    {
        if (Event.current.type == EventType.keyDown)
        {
            KeyCode keyCode = Event.current.keyCode;

            if (keyCode == KeyCode.Q)
            {
                Event.current.Use();
            }

            else if(keyCode == KeyCode.W)
            {
                SwapToolMode();
                window.Repaint();
                Event.current.Use();
            }

            else if(keyCode == KeyCode.E)
            {
                Event.current.Use();
            }

            else if (keyCode == KeyCode.R)
            {
                Event.current.Use();
            }

            else if (keyCode == KeyCode.T)
            {
                Event.current.Use();
            }
        }
    }

    private void ProcessMouseEvents(SceneView sceneView)
    {
        if (!ObjectPlacerWindow.selectedLayer || ObjectPlacerWindow.placableObjects == null)
            return;

        if (Event.current.button == 0 || Event.current.button == 1)
        {
            if (toolMode == ToolMode.Single)
                HandleSingleToolMode();
            else
                HandleRectangleToolMode(sceneView);
        }
    }

    private void HandleRectangleToolMode(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown)
        {
            rectangleStart = GetMousePos();
            Event.current.Use();
        }

        else if(Event.current.type == EventType.MouseDrag)
        {
            drawRect = true;
            
            Vector2 curPos = GetMousePos();
            Vector2 roundedCurPos = ObjectPlacerWindow.selectedLayer.RoundToClosestPointOnGrid(rectangleStart);

            Vector2 roundedEndPos = ObjectPlacerWindow.selectedLayer.RoundToClosestPointOnGrid(curPos);

            minRectPos = new Vector2(Mathf.Min(roundedCurPos.x, roundedEndPos.x), Mathf.Min(roundedCurPos.y, roundedEndPos.y));
            maxRectPos = new Vector2(Mathf.Max(roundedCurPos.x, roundedEndPos.x), Mathf.Max(roundedCurPos.y, roundedEndPos.y));
            
            Event.current.Use();
        }

        else if(Event.current.type == EventType.MouseUp)
        {
            drawRect = false;

            Vector2 endPos = GetMousePos();
            Vector2 roundedStartPos;
            Vector2i startId = ObjectPlacerWindow.selectedLayer.CreateIndexFromPos(rectangleStart, out roundedStartPos);

            Vector2 roundedEndPos;
            Vector2i endId = ObjectPlacerWindow.selectedLayer.CreateIndexFromPos(endPos, out roundedEndPos);

            int minX = Mathf.Min(startId.x, endId.x);
            int maxX = Mathf.Max(startId.x, endId.x);
            int minY = Mathf.Min(startId.y, endId.y);
            int maxY = Mathf.Max(startId.y, endId.y);

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    Vector2i index = new Vector2i(i, j);
                    ModifyObject(index, ObjectPlacerWindow.selectedLayer.GetClosestPointFromIndex(index));
                }
            }

            Event.current.Use();
        }

    }

    private Vector2 GetMousePos()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        return new Vector2(ray.origin.x, ray.origin.y);
    }

    private void ModifyObject(Vector2i intPos, Vector2 roundedWorldPos)
    {
        if (Event.current.button == 0)
            ObjectPlacerWindow.selectedLayer.TryPlaceObjectAt(ObjectPlacerWindow.placableObjects, roundedWorldPos, intPos, false, true);
       
        else if(Event.current.button == 1)
            ObjectPlacerWindow.selectedLayer.DestroyObjectAt(ObjectPlacerWindow.placableObjects, intPos);
  
    }

    private void HandleSingleToolMode()
    {
        if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseUp)
            return;

        Vector2 mousePos = GetMousePos();
        Vector2 closestPos;
        Vector2i intPos = ObjectPlacerWindow.selectedLayer.CreateIndexFromPos(mousePos, out closestPos);

        ModifyObject(intPos, closestPos);

        Event.current.Use();
    }

    private void OnGUI()
    {
        ObjectPlacerWindow.selectedLayer = (ObjectLayer)EditorGUILayout.ObjectField("ObjectLayer Root:", ObjectPlacerWindow.selectedLayer, typeof(ObjectLayer), true);
        ObjectPlacerWindow.placableObjects = (ObjectSet)EditorGUILayout.ObjectField("Placable Objects:", ObjectPlacerWindow.placableObjects, typeof(ObjectSet), false);

        ObjectPlacerWindow.toolMode = (ToolMode)EditorGUILayout.EnumPopup("ToolMode (W)", ObjectPlacerWindow.toolMode);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (ObjectPlacerWindow.placableObjects != null)
        {
            Editor editor = Editor.CreateEditor(placableObjects);
            editor.DrawDefaultInspector();
        }
    }

    private void SwapToolMode()
    {
        if (toolMode == ToolMode.Rectangle)
            toolMode = ToolMode.Single;
        else
            toolMode = ToolMode.Rectangle;
    }

    public static void DrawWireRectangle(Vector2 min, Vector2 max)
    {
        DrawWireRectangle(min.x, min.y, max.x, max.y);
    }

    public static void DrawWireRectangle(float minX, float minY, float maxX, float maxY)
    {
        Handles.DrawLine(new Vector3(minX, minY, 0.0f), new Vector3(maxX, minY, 0.0f));
        Handles.DrawLine(new Vector3(maxX, minY, 0.0f), new Vector3(maxX, maxY, 0.0f));
        Handles.DrawLine(new Vector3(maxX, maxY, 0.0f), new Vector3(minX, maxY, 0.0f));
        Handles.DrawLine(new Vector3(minX, maxY, 0.0f), new Vector3(minX, minY, 0.0f));
    }

}
