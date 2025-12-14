using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleNotificaterCallerBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private List<Renderer> inspectThisMesh;
    [SerializeField] private List<string> message;

    public void Inspect()
    {
        ActorControlTypeStateMachine.PushStateToPopUpNotif(message);
    }

    public void ShowHighlight(Material material)
    {
        foreach (Renderer inspectMesh in inspectThisMesh)
        {
            int length = inspectMesh.materials.Length;
            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = 0; i < length; i++) materials.Insert(i * 2 + 1, material);
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
}
