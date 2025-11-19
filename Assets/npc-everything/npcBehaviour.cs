using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NpcBehaviour : MonoBehaviour, IConversable
{
    [SerializeField] private List<PuzzleData> puzzles;
    [SerializeField] private UnityEvent rair;
    private ReadOnlyCollection<string> currentChitChat;
    private PuzzleData currentPuzzleData;
    private float currentTimeInSeconds;
    private int indexChitChat;


    // delegate of function?
    private void Start()
    {
    }

    private void Update()
    {
    }

    public bool StartChitChat()
    {
        currentPuzzleData = GetCurrentPuzzle(puzzles);
        indexChitChat = 0;

        if (currentPuzzleData.IsUnityNull())
        {
            currentPuzzleData = puzzles[^1];
            return false;
        }

        currentChitChat = currentPuzzleData.GetDialogNormalChitChat();


        PlayChitChat(currentChitChat[indexChitChat]);
        return true;
    }

    public void StartResponseIsCorrectChitChat()
    {
        indexChitChat = 0;
        currentChitChat = currentPuzzleData.GetDialogPuzzleSolvedChitChat();
        PlayChitChat(currentChitChat[indexChitChat]);
    }

    public void StartResponseIsWrongChitChat()
    {
        indexChitChat = 0;
        currentChitChat = currentPuzzleData.GetDialogResponseFalseChitChat();
        PlayChitChat(currentChitChat[indexChitChat]);
    }

    public void StartTimeRanOutChitChat()
    {
        indexChitChat = 0;
        currentChitChat = currentPuzzleData.GetDialogTimeRunOutChitChat();
        PlayChitChat(currentChitChat[indexChitChat]);
    }


    public bool NextChitChat()
    {
        if (indexChitChat + 1 >= currentChitChat.Count)
        {
            PlayChitChat("");
            return false;
        }

        indexChitChat += 1;
        PlayChitChat(currentChitChat[indexChitChat]);

        return true;
    }

    public bool StartTalkPrompt()
    {
        currentPuzzleData = GetCurrentPuzzle(puzzles);
        indexChitChat = 0;

        if (currentPuzzleData.IsUnityNull())
        {
            currentPuzzleData = puzzles[^1];
            return false;
        }

        UIController.InsertPromptTextForTMP(currentPuzzleData.GetPuzzlePrompt());
        currentTimeInSeconds = currentPuzzleData.timeLimit;
        // TODO:
        // - Activate time limit?
        return true;
    }

    public bool TryResponse(List<WordData> tryWords)
    {
        Debug.Log("Enter TryResponse(List<WordData>) of: " + gameObject.name);
        if (tryWords.Count != currentPuzzleData.GetSolutionWords().Count) return false;

        for (int i = 0; i < tryWords.Count; i++)
        {
            if (tryWords[i].Equals(currentPuzzleData.GetSolutionWords()[i])) continue;
            Debug.Log("Words are false");
            return false;
        }

        currentPuzzleData.GetSolutionSentence().SetSentence();
        Debug.Log(currentPuzzleData.GetSolutionSentence().finalSentence);

        currentPuzzleData.SetIsSolved(true);
        UIController.InsertPromptTextForTMP("");
        Debug.Log("Words are correct");
        return true;
    }

    public SentenceData GetSolutionSentence()
    {
        Debug.Log(currentPuzzleData.GetSolutionSentence().tense);
        return currentPuzzleData.GetSolutionSentence();
    }

    public float GetTimeLimitCurrentPuzzle()
    {
        return currentPuzzleData.timeLimit;
    }

    public void Inspect()
    {
        Debug.Log("You inspected " + gameObject.name);
    }

    private PuzzleData GetCurrentPuzzle(List<PuzzleData> localPuzzles, int index = 0)
    {
        if (index >= localPuzzles.Count) return null;

        return localPuzzles[index].IsPuzzleSolved() ? GetCurrentPuzzle(localPuzzles, index + 1) : localPuzzles[index];
    }

    private void PlayChitChat(string playThis)
    {
        // have string appear every character per frame?
        UIController.InsertTextForTMP(playThis);
    }
}
