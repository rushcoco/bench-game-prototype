using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private Renderer gameObjectWithRenderer;
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

    public void ShowHighlight(Material material)
    {
        // bool hasNoOutlines = true;
        // foreach (Material material1 in gameObjectWithRenderer.materials)
        // {
        //     if (material1 == material)
        //         hasNoOutlines = false;
        // }
        //
        // if (hasNoOutlines)
        // {
        //     List<Material> materials = gameObjectWithRenderer.materials.ToList();
        //     for (int i = 0; i < gameObjectWithRenderer.materials.Length; i++)
        //     {
        //         if ()
        //     }
        // }
    }

    public void HideHighlight(Material material)
    {
    }
}
