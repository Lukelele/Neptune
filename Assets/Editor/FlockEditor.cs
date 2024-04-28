using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Flock))]
public class FlockEditor : Editor
{
    
    private int numOfUnit =1;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        // text field for the number of units
        EditorGUILayout.LabelField("Create new Units", EditorStyles.boldLabel);
        Flock flock = (Flock) target;
        numOfUnit = EditorGUILayout.IntField("Number of Units", numOfUnit);
        
        // button to create the units
        if (GUILayout.Button("Create Units"))
        {
            for (int i = 0; i < numOfUnit; i++)
            {
                flock.CreateUnit();
            }
        }
        
    }
}
