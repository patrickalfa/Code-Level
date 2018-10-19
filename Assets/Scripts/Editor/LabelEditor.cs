#if UNITY_EDITOR

using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Label))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Label label = (Label)target;
        if (GUILayout.Button("Update"))
            label.UpdateText();
    }
}

#endif