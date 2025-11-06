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

    public bool TryCraftWords(List<NounData> nouns, out VerbData verb)
    {
        foreach (CraftData craftData in allCraftableData)
        {
            foreach (NounData craftDataCraftWord in craftData.craftWords)
                Debug.Log("Found Word: " + craftDataCraftWord.presentedWord);
            Debug.Log("Fouond Verb: " + craftData.craftedWord.presentedWord);
        }

        verb = null;
        // TODO:
        // Check if the List of Words/ Words Combination exist
        // return true and push stack to "learned word"
        // return false and push stack to "wrong"
        List<NounData> throwAwayList = new();
        foreach (NounData nounData in nouns)
        {
            if (throwAwayList.Contains(nounData))
                return false;

            throwAwayList.Add(nounData);
        }

        throwAwayList.Clear();
        throwAwayList.TrimExcess();

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

    public static CraftableManager Instance()
    {
        if (instance == null)
            instance = new CraftableManager();

        return instance;
    }
}
