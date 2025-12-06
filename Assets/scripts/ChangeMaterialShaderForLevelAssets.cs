#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMaterialShaderForLevelAssets
{
    private const string ToolPath = "Tools/Remap MAterials In Open Scenes";
    
    [MenuItem(ToolPath)]
    public static void RemapAndSave()
    {
        var helper = Object.FindFirstObjectByType<ChangeMaterialShaderForObstacles>();
        if (helper == null)
        {
            Debug.LogError("Rair ad a changeMaterialShaaderForLEvelAssets component in the scene");
        }
        
        helper.RemapMaterials();

        string scenePath = EditorUtility.OpenFilePanel("Select Open Scene","", "unity");

        Scene openedScene = EditorSceneManager.GetSceneByPath(scenePath);
        EditorSceneManager.MarkSceneDirty(openedScene);
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();

    }
}

#endif
