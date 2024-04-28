using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MatController))]
public class MatControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MatController matController = (MatController) target;
        if (GUILayout.Button("Set Transparent"))
        {
            matController.ToggleTransparency(false);
        }
        if (GUILayout.Button("Set Not Transparent"))
        {
            matController.ToggleTransparency(true);
        }
        
    }
}