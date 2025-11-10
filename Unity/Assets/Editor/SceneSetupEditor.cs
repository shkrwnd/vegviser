using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for BuildingSceneSetup to add context menu
/// </summary>
[CustomEditor(typeof(BuildingSceneSetup))]
public class SceneSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        BuildingSceneSetup setup = (BuildingSceneSetup)target;
        
        if (GUILayout.Button("Generate Building Scene"))
        {
            setup.GenerateBuildingScene();
        }
    }
}

