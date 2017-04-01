using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ObjectLayer))]
public class ObjectLayerInspector : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Clear Layer"))
        {
            ObjectLayer layer = (ObjectLayer)this.target;
            layer.Clear();
        }
    }
}
