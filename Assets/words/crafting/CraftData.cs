using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftData", menuName = "Scriptable Objects/CraftData")]
public class CraftData : ScriptableObject
{
    public int id;
    public List<NounData> craftWords;
    public VerbData craftedWord;

    private void OnEnable()
    {
        CraftableManager instance = CraftableManager.Instance();
        if (instance.AddCraftable(this)) id = instance.getCraftData.Count - 1;
    }
}
