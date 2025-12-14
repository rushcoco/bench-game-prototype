using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleNotificaterCallerBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private List<Renderer> inspectThisMesh;
    [SerializeField] private List<string> message;

    private List<Material> instanceMaterials;

    private void Awake()
    {
        instanceMaterials = new List<Material>();
    }

    public void Inspect()
    {
        ActorControlTypeStateMachine.PushStateToPopUpNotif(message);
    }

    public void ShowHighlight(Material material)
    {
        foreach (Renderer inspectMesh in inspectThisMesh)
        {
            // if (CheckForHighlight(inspectMesh.materials, material)) continue;

            int length = inspectMesh.materials.Length;
            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = 0; i < length; i++) materials.Insert(i * 2 + 1, material);
            // instanceMaterials.Add(materials[i * 2 + 1]);
            inspectMesh.materials = materials.ToArray();

            materials.Clear();
            materials.TrimExcess();
        }
    }

    public void HideHighlight(Material material)
    {
        foreach (Renderer inspectMesh in inspectThisMesh)
        {
            // if (!CheckForHighlight(inspectMesh.materials, material)) continue;

            int length = Mathf.FloorToInt(inspectMesh.materials.Length * 0.5f);

            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = length; i > 0; i--)
                // instanceMaterials.Remove(materials[i * 2 - 1]);
                materials.RemoveAt(i * 2 - 1);

            inspectMesh.materials = materials.ToArray();
        }
    }

    private bool CheckForHighlight(Material[] materials, Material compareTo)
    {
        return materials.Any(material => instanceMaterials.Any(material.Equals));
    }
}
