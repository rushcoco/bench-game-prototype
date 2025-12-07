#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ChangeMaterialShaderForLevelAssets
{
    private const string TOOL_PATH = "Tools/Remap MAterials In Open Scenes";
    
    [MenuItem(TOOL_PATH)]
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
