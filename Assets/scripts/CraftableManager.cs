using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CraftableManager
{
    private static CraftableManager instance;
    private readonly Dictionary<Hash128, VerbData> craftedWords;
    private List<CraftData> allCraftableData;

    private CraftableManager()
    {
        allCraftableData = new List<CraftData>();
        craftedWords = new Dictionary<Hash128, VerbData>();
    }

    public IReadOnlyCollection<CraftData> getCraftData => allCraftableData;

    public void AddCraftable(NounData word1, NounData word2, VerbData resultWord)
    {
        Hash128 keyCombo = Hash128.Compute(word1.presentedWord + word2.presentedWord);
        if (keyCombo.isValid)
            craftedWords.Add(keyCombo, resultWord);
        else
            throw new EvaluateException();
    }

    public bool AddCraftable(CraftData craftData)
    {
        allCraftableData ??= new List<CraftData>();
        if (allCraftableData.Contains(craftData)) return false;

        allCraftableData.Add(craftData);
        return true;
    }

    public static CraftableManager Instance()
    {
        if (instance == null)
            instance = new CraftableManager();

        return instance;
    }
}
