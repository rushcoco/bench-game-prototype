using System.Collections.Generic;
using System.Linq;
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

    public static CraftableManager Instance()
    {
        if (instance == null)
            instance = new CraftableManager();

        return instance;
    }

    public bool AddCraftable(CraftData craftData)
    {
        allCraftableData ??= new List<CraftData>();
        if (allCraftableData.Contains(craftData)) return false;

        allCraftableData.Add(craftData);
        return true;
    }

    public bool TryCraftWords(List<NounData> nouns, out VerbData verb)
    {
        verb = null;

        HashSet<NounData> hashSetToCheckIfSomeNounsAreDuplicated = new();

        if (nouns.Any(noun => !hashSetToCheckIfSomeNounsAreDuplicated.Add(noun)))
            return false;

        hashSetToCheckIfSomeNounsAreDuplicated.Clear();
        hashSetToCheckIfSomeNounsAreDuplicated.TrimExcess();

        foreach (CraftData data in allCraftableData)
            for (int i = 0; i < nouns.Count; i++)
            {
                if (!data.craftWords.Contains(nouns[i])) break;
                if (i < nouns.Count - 1) continue;

                verb = data.craftedWord;
                return true;
            }


        return false;
    }
}
