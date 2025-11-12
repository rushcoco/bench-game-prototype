using System.Collections.Generic;
using UnityEngine;

public class LearnWordRewardHandler : RewardHandler
{
    [SerializeField] private List<WordData> learnThisWord;

    protected override void HandlePuzzleSolved()
    {
        foreach (WordData wordData in learnThisWord) ActorManager.TryAddWordToWordsCollected(wordData);
        
        
    }
}
