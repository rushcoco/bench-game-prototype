using System.Collections.Generic;
using UnityEngine;

public class LearnWordRewardHandler : RewardHandler
{
    [SerializeField] private List<WordData> learnThisWord;
    private List<string> messages;

    protected override void HandlePuzzleSolved()
    {
        // TODO: Remove String Notations and make it to a serialized field.
        Debug.Log("Enter Handle Puzzle Solved");

        messages = new List<string>();
        foreach (WordData word in learnThisWord)
            if (ActorManager.TryAddWordToWordsCollected(word))
                messages.Add($"You have learned the word '{word.presentedWord}'.");
            else
                messages.Add($"You already learned the word '{word.presentedWord}'");
        ActorControlTypeStateMachine.PushStateToPopUpNotif(messages);
        messages.Clear();
        messages.TrimExcess();
    }
}
