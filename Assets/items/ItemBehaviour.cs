using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private List<ItemData> itemData;
    private List<string> messages;

    public void Inspect()
    {
        messages = new List<string>();
        foreach (ItemData data in itemData)
            if (ActorManager.TryAddWordToWordsCollected(data.learnThisWord))
                messages.Add($"You have learned the word '{data.learnThisWord.presentedWord}'.");
            else
                messages.Add($"You already learned the word '{data.learnThisWord.presentedWord}'");
        ActorControlTypeStateMachine.PushStateToPopUpNotif(messages);
        messages.Clear();
        messages.TrimExcess();
    }
}
