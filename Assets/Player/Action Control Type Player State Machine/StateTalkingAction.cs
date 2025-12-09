using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StateTalkingAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputClickOnThings;
    [SerializeField] private InputActionReference inputCursorPosition;
    [SerializeField] private InputActionReference inputRemoveLastWordFromSentence;
    [SerializeField] private RectTransform sentenceContainerRectangle;
    [SerializeField] private RectTransform wordSelectorRectangle;
    [SerializeField] private GameObject emptyUIGameObject;
    [SerializeField] private RectTransform sentenceBuildingCanvas;
    [SerializeField] private float secondsUntilClickInputIsRegisteredAsHolding;

    private IConversable currentConversable;

    // Sentence Building Logic Variables
    private List<WordBehaviour> currentSentence;
    private float currentTimeInSeconds;
    private List<WordBehaviour> currentWordsThatCanBeSelected;

    // Drag Behaviour Variable
    private WordBehaviour draggedWord;
    private CanvasGroup draggedWordCanvasGroup;
    private Vector2 dragOffset;

    private bool inputClickQueued;
    private Transform originalParent;
    private float secondsMouseClickBeingHeld;
    private Pronoun solutionPronoun;
    private Tense solutionTense;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (currentTimeInSeconds <= 0f)
        {
            // TODO:
            // - Have the Dialog play the time run out dialog
            currentConversable.StartTimeRanOutChitChat();
            ActorControlTypeStateMachine.ChangeStateToListening(currentConversable);
        }
        else
        {
            currentTimeInSeconds -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        sentenceBuildingCanvas.gameObject.SetActive(true);

        inputClickOnThings.action.Enable();
        inputCursorPosition.action.Enable();
        inputRemoveLastWordFromSentence.action.Enable();

        inputRemoveLastWordFromSentence.action.performed += OnInputActionPerformedRemoveLastWordFromTheSentence;

        inputClickOnThings.action.started += OnInputActionStartedClickOnThings;
        inputClickOnThings.action.canceled += OnInputActionCanceledClickOnThings;

        currentSentence = new List<WordBehaviour>();
        currentWordsThatCanBeSelected = new List<WordBehaviour>();

        ActorControlTypeStateMachine.SetCursorModes(true, CursorLockMode.None);

        foreach (WordData word in ActorManager.GetAllWordsPlayerHasCollected())
        {
            WordBehaviour currEmptyWord =
                Instantiate(emptyUIGameObject, wordSelectorRectangle).AddComponent<WordBehaviour>();
            currEmptyWord.SetWord(word);
            currEmptyWord.GetComponent<TextMeshProUGUI>().text = currEmptyWord.wordData.presentedWord;

            currentWordsThatCanBeSelected.Add(currEmptyWord);
        }

        solutionTense = currentConversable.GetSolutionSentence().tense;
        solutionPronoun = currentConversable.GetSolutionSentence().pronoun;

        inputClickQueued = false;
        secondsMouseClickBeingHeld = 0f;
    }

    private void OnDisable()
    {
        inputClickQueued = false;
        secondsMouseClickBeingHeld = 0f;

        ActorControlTypeStateMachine.SetCursorModes(false, CursorLockMode.Locked);

        // currentConversable = null;

        currentWordsThatCanBeSelected.ForEach(behaviour => Destroy(behaviour.gameObject));
        currentSentence.ForEach(behaviour => Destroy(behaviour.gameObject));

        currentWordsThatCanBeSelected.Clear();
        currentSentence.Clear();

        currentWordsThatCanBeSelected.TrimExcess();
        currentSentence.TrimExcess();

        inputClickOnThings.action.started -= OnInputActionStartedClickOnThings;
        inputClickOnThings.action.canceled -= OnInputActionCanceledClickOnThings;

        inputRemoveLastWordFromSentence.action.performed -= OnInputActionPerformedRemoveLastWordFromTheSentence;

        inputRemoveLastWordFromSentence.action.Disable();
        inputClickOnThings.action.Disable();
        inputCursorPosition.action.Disable();

        sentenceBuildingCanvas.gameObject.SetActive(false);
    }

    public void ExitState()
    {
        enabled = false;
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void OnInputActionStartedClickOnThings(InputAction.CallbackContext context)
    {
        inputClickQueued = true;
        StartCoroutine(CountingSeconds());
    }

    private void OnInputActionCanceledClickOnThings(InputAction.CallbackContext context)
    {
        Debug.Log("Click Time Held: " + secondsMouseClickBeingHeld);
        if (inputClickQueued && secondsMouseClickBeingHeld >= secondsUntilClickInputIsRegisteredAsHolding)
        {
            // The player is holding the mouse ->
            // Check if a word is being held ->
            // Check if the word is in a new position
            // Get that word in between the other words in the new position.
        }
        else if (inputClickQueued && secondsMouseClickBeingHeld < secondsUntilClickInputIsRegisteredAsHolding)
        {
            // Player simply clicked on smth
            // Do the click input
            ActorPerformedShortClick();
        }
        // At the very end queued gets defaulted

        inputClickQueued = false;
        secondsMouseClickBeingHeld = 0f;
    }

    private void ActorPerformedShortClick()
    {
        if (!IsWordClickedOn(out WordBehaviour foundWord)) return;

        Debug.Log("Found WordData: " + foundWord.wordData.presentedWord);
        if (currentSentence.Contains(foundWord))
        {
            currentSentence.Remove(foundWord);
            Destroy(foundWord.gameObject);
        }
        else
        {
            currentSentence.Add(Instantiate(foundWord, sentenceContainerRectangle));
        }

        WordPositionsInSentence();
    }

    private void OnInputActionPerformedRemoveLastWordFromTheSentence(InputAction.CallbackContext context)
    {
        if (currentSentence.IsUnityNull()) return;
        if (currentSentence.Count < 1) return;

        WordBehaviour word = currentSentence[^1];
        currentSentence.RemoveAt(currentSentence.Count - 1);
        Destroy(word.gameObject);
    }

    private bool IsWordClickedOn(out WordBehaviour foundWord)
    {
        foundWord = null;
        Vector2 mousePosition = Mouse.current.position.value;

        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult raycastResult in results)
        {
            if (!raycastResult.gameObject.TryGetComponent(out WordBehaviour word)) continue;

            foundWord = word;
            return true;
        }

        return false;
    }

    private void WordPositionsInSentence()
    {
        foreach (WordBehaviour wordBehaviour in currentSentence)
        {
            if (wordBehaviour.wordData is not VerbData verbData) continue;
            if (wordBehaviour.TryGetComponent(out TextMeshProUGUI tmpUGUI))
                tmpUGUI.text = VerbConjugator.Conjugate(verbData, solutionTense, solutionPronoun);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainerRectangle);
    }

    public void SetIConversable(IConversable conversable)
    {
        currentConversable = conversable;
        currentTimeInSeconds = conversable.GetTimeLimitCurrentPuzzle();
    }

    public void OnTimeRunOut()
    {
    }

    public void OnRespond()
    {
        Debug.Log("Button Clicked OnRespond");
        // currentSentence.ConvertAll(x => x.wordData).ForEach(x => Debug.Log(x.presentedWord));
        if (currentConversable.TryResponse(currentSentence.ConvertAll(x => x.wordData)))
        {
            currentConversable.StartResponseIsCorrectChitChat();
            ActorControlTypeStateMachine.ChangeStateToListening(currentConversable);
        }
        else
        {
            currentConversable.StartResponseIsWrongChitChat();
            ActorControlTypeStateMachine.PushStateToListening(currentConversable);
        }
    }

    private IEnumerator CountingSeconds()
    {
        secondsMouseClickBeingHeld = 0f;

        while (inputClickQueued)
        {
            secondsMouseClickBeingHeld += Time.deltaTime;
            yield return null;
        }
    }
}
