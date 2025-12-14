using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private List<Renderer> inspectThisMesh;
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
        foreach (Renderer inspectMesh in inspectThisMesh)
        {
            int length = inspectMesh.materials.Length;
            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = 0; i < length; i++)
                materials.Insert(i * 2 + 1, material);

            inspectMesh.materials = materials.ToArray();

            materials.Clear();
            materials.TrimExcess();
        }
    }

    public void HideHighlight(Material material)
    {
        foreach (Renderer inspectMesh in inspectThisMesh)
        {
            int length = Mathf.FloorToInt(inspectMesh.materials.Length * 0.5f);

            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = length; i > 0; i--)
                materials.RemoveAt(i * 2 - 1);

            inspectMesh.materials = materials.ToArray();
        }
    }

    private bool CheckForHighlight(Material[] materials, Material compareTo)
    {
        return materials.Any(material => material.Equals(compareTo));
    }
}
