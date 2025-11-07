using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Scriptable Objects/PuzzleData")]
public class PuzzleData : ScriptableObject
{
    // SentenceData
    [HideInInspector] public int id;
    [SerializeField] public SentenceData correctSentenceData;
    [SerializeField] public List<string> dialogChitChat;

    [SerializeField] public float timelimitForPuzzleInSeconds;
    [SerializeField] public List<string> dialogPuzzleSolved;
    [SerializeField] public string dialogResponseFalse;
    [SerializeField] public string dialogTimeRunOut;
    [SerializeField] public string dialogPuzzlePrompt;
    [SerializeField] private UnityEvent rewardForThePlayer;

    // isPuzzleSolved?
    [SerializeField] private bool isActuallySolved;
    public bool isSolved => isActuallySolved;
    public float timeLimit => timelimitForPuzzleInSeconds;

    public IReadOnlyList<WordData> GetSolutionWords()
    {
        List<WordData> correctWords = new();
        foreach (WordData sentenceWord in correctSentenceData.words) correctWords.Add(sentenceWord);
        return correctWords;
    }

    public SentenceData GetSolutionSentence()
    {
        return correctSentenceData;
    }

    public ReadOnlyCollection<string> GetTextForWhenPuzzleIsSolved()
    {
        return dialogPuzzleSolved.AsReadOnly();
    }

    public int GetCountDialogPuzzleSolved()
    {
        return dialogPuzzleSolved.Count;
    }

    public string GetChitChat(int index)
    {
        return dialogChitChat[index];
    }

    public int GetChitChatCount()
    {
        return dialogChitChat.Count;
    }

    public string GetPuzzlePrompt()
    {
        return dialogPuzzlePrompt;
    }

    public void SetIsSolved(bool value)
    {
        if (value)
            rewardForThePlayer?.Invoke();
        isActuallySolved = value;
    }
}
