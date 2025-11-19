using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Scriptable Objects/PuzzleData")]
public class PuzzleData : ScriptableObject
{
    // SentenceData
    [HideInInspector] public int id;
    [SerializeField] public SentenceData correctSentenceData;
    [SerializeField] public List<string> dialogChitChat;

    [SerializeField] public float timeLimitInSeconds;
    [SerializeField] public List<string> dialogPuzzleSolved;
    [SerializeField] public string dialogResponseFalse;
    [SerializeField] public string dialogTimeRunOut;
    [SerializeField] public string dialogPuzzlePrompt;

    // isPuzzleSolved?
    [SerializeField] private bool isPuzzleSolved;
    public float timeLimit => timeLimitInSeconds;
    public event Action OnPuzzleSolved;

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

    public ReadOnlyCollection<string> GetDialogPuzzleSolvedChitChat()
    {
        return dialogPuzzleSolved.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetDialogResponseFalseChitChat()
    {
        List<string> dialogResponseFalseChitChats = new() { dialogResponseFalse };
        return dialogResponseFalseChitChats.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetDialogTimeRunOutChitChat()
    {
        List<string> dialogResponseFalseChitChats = new() { dialogTimeRunOut };
        return dialogResponseFalseChitChats.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetDialogNormalChitChat()
    {
        return dialogChitChat.AsReadOnly();
    }

    public int GetCountDialogPuzzleSolved()
    {
        return dialogPuzzleSolved.Count;
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
            OnPuzzleSolved?.Invoke();
        isPuzzleSolved = value;
    }

    public bool IsPuzzleSolved()
    {
        return isPuzzleSolved;
    }
}
