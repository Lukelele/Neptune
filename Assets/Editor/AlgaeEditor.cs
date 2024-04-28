using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Algae))]
public class AlgaeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Algae algae = (Algae) target;
        if (GUILayout.Button("Bind Child"))
        {
            algae.BindChild();
        }
        
    }
}