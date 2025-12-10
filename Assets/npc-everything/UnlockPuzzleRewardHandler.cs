using System.Collections.Generic;
using UnityEngine;

public class UnlockPuzzleRewardHandler : RewardHandler
{
    [SerializeField] private List<PuzzleData> unlockThisReward;
    [SerializeField] private NpcBehaviour ownerOfUnlockedPuzzles;

    protected override void HandlePuzzleSolved()
    {
        // If Reward gets triggered then
        // get Reward Puzzles and the NPC of who will unlock it
        foreach (PuzzleData puzzleData in unlockThisReward) ownerOfUnlockedPuzzles.AddPuzzleDataToPuzzles(puzzleData);
    }
}
