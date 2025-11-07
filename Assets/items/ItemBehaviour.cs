using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private ItemData itemData;

    public void Inspect()
    {
        // Player should learn this word
        if (ActorManager.TryAddWordToWordsCollected(itemData.learnThisWord))
        {
            ActorControlTypeStateMachine.PushStateToPopUpNotif(
                $"You have learned the word '{itemData.learnThisWord.presentedWord}'.");
            Renderer thisrend = GetComponent<Renderer>();
            thisrend.enabled = false;
            return;
        }

        ActorControlTypeStateMachine.PushStateToPopUpNotif(
            $"You already learned the word '{itemData.learnThisWord.presentedWord}'");
    }
}
