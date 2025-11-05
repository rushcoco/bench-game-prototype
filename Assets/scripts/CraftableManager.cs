using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CraftableManager
{
    public Dictionary<Hash128, VerbData> craftedWords;

    public void AddCraftable(NounData word1, NounData word2, VerbData resultWord)
    {
        Hash128 keyCombo = Hash128.Compute(word1.presentedWord + word2.presentedWord);
        if (keyCombo.isValid)
            craftedWords.Add(keyCombo, resultWord);
        else
            throw new EvaluateException();
    }
}
