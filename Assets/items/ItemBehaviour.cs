using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private ItemData itemData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

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
