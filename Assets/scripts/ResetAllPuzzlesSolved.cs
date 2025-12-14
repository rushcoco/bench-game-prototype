using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ResetAllPuzzlesSolved
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reset Puzzles")]
#endif
    public static void ResetPuzzlesSolved()
    {
        Scene openScene = SceneManager.GetActiveScene();

        IEnumerable<GameObject> gameObjects =
            openScene.GetRootGameObjects().Where(gameObject => gameObject.CompareTag("NPC"));

        foreach (GameObject gameObject in gameObjects)
            if (gameObject.TryGetComponent(out NpcBehaviour npcBehaviour))
            {
                npcBehaviour.GetPuzzleData().ForEach(puzzle => puzzle.SetIsSolved(false));

                if (!gameObject.TryGetComponent(out UnlockPuzzleRewardHandler _)) continue;

                UnlockPuzzleRewardHandler[] rewardHandlers = gameObject.GetComponents<UnlockPuzzleRewardHandler>();

                foreach (UnlockPuzzleRewardHandler unlockPuzzleRewardHandler in rewardHandlers)
                    unlockPuzzleRewardHandler.GetRewardPuzzles().ForEach(data => data.SetIsSolved(false));
            }
    }
}
