#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class ResetAllPuzzlesSolved
{
    // Get Open Scene
    // Find Open Game Objects with Tag "NPC"
    [MenuItem("Tools/Reset Puzzles")]
    public static void ResetPuzzlesSolved()
    {
        Scene openScene = SceneManager.GetActiveScene();

        var gameObjects = openScene.GetRootGameObjects().Where(gameObject => gameObject.CompareTag("NPC"));

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent<NpcBehaviour>(out NpcBehaviour npcBehaviour))
                npcBehaviour.GetPuzzleData().ForEach(puzzle => puzzle.SetIsSolved(false));
        }
    }
}
#endif
