using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NpcBehaviour : MonoBehaviour, IConversable
{
    [SerializeField] private List<PuzzleData> puzzles;
    private PuzzleData currentPuzzleData;
    private float currentTimeInSeconds;
    private int indexChitChat;
    [SerializeField] private UnityEvent onAfterPuzzleSolved;


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


        PlayChitChat(currentPuzzleData.GetChitChat(indexChitChat));
        return true;
    }

    public bool NextChitChat()
    {
        if (currentPuzzleData.isSolved)
        {
            if (indexChitChat + 1 >= currentPuzzleData.GetCountDialogPuzzleSolved())
            {
                PlayChitChat("");
                return false;
            }

            indexChitChat += 1;
            PlayChitChat(currentPuzzleData.GetTextForWhenPuzzleIsSolved()[indexChitChat]);
        }
        else
        {
            if (indexChitChat + 1 >= currentPuzzleData.GetChitChatCount())
            {
                PlayChitChat("");
                return false;
            }

            indexChitChat += 1;
            PlayChitChat(currentPuzzleData.GetChitChat(indexChitChat));
        }

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

    public void StartSolutionChitChat()
    {
        indexChitChat = 0;
        PlayChitChat(currentPuzzleData.GetTextForWhenPuzzleIsSolved()[indexChitChat]);
        
        if(GetCurrentPuzzle(puzzles) == null)
            onAfterPuzzleSolved?.Invoke();
    }

    public void StartWrongResponseChitChat()
    {
        ActorControlTypeStateMachine.PushStateToPopUpNotif(currentPuzzleData.dialogResponseFalse);
    }

    public SentenceData GetSolutionSentence()
    {
        Debug.Log(currentPuzzleData.GetSolutionSentence().tense);
        return currentPuzzleData.GetSolutionSentence();
    }

    public void Inspect()
    {
        Debug.Log("You inspected " + gameObject.name);
    }

    private PuzzleData GetCurrentPuzzle(List<PuzzleData> localPuzzles, int index = 0)
    {
        if (index >= localPuzzles.Count) return null;

        return localPuzzles[index].isSolved ? GetCurrentPuzzle(localPuzzles, index + 1) : localPuzzles[index];
    }

    private void PlayChitChat(string playThis)
    {
        // have string appear every character per frame?
        UIController.InsertTextForTMP(playThis);
    }
}
