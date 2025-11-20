using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StateCraftingWordsAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputClickOnThings;
    [SerializeField] private InputActionReference inputCursorPosition;
    [SerializeField] private RectTransform craftingWordsCanvas;
    [SerializeField] private RectTransform craftingWordsTablePanel;
    [SerializeField] private RectTransform craftWordsSelectorPanel;
    [SerializeField] private int amountOfNounsThatCanBeCraftedInTotal;

    [SerializeField] private GameObject emptyUIGameObject;
    private List<WordBehaviour> toBeCrafted;
    private List<WordBehaviour> wordsThatCanBeSelected;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        craftingWordsCanvas.gameObject.SetActive(true);

        ActorManager.PlayOneShotOnEnterCraftingMenu();

        inputClickOnThings.action.Enable();
        inputCursorPosition.action.Enable();

        inputClickOnThings.action.performed += OnInputActionPerformedClickOnThings;

        ActorControlTypeStateMachine.SetCursorModes(true, CursorLockMode.None);

        wordsThatCanBeSelected = new List<WordBehaviour>();
        toBeCrafted = new List<WordBehaviour>();

        foreach (NounData nounData in ActorManager.GetAllNounsPlayerHasCollected())
        {
            WordBehaviour currEmptyWord =
                Instantiate(emptyUIGameObject, craftWordsSelectorPanel).AddComponent<WordBehaviour>();
            currEmptyWord.SetWord(nounData);
            currEmptyWord.GetComponent<TextMeshProUGUI>().text = currEmptyWord.wordData.presentedWord;

            wordsThatCanBeSelected.Add(currEmptyWord);
        }
    }

    private void OnDisable()
    {
        ActorControlTypeStateMachine.SetCursorModes(false, CursorLockMode.Locked);

        inputClickOnThings.action.performed -= OnInputActionPerformedClickOnThings;

        inputClickOnThings.action.Disable();
        inputCursorPosition.action.Disable();

        toBeCrafted.ForEach(data => Destroy(data.gameObject));
        toBeCrafted.Clear();
        toBeCrafted.TrimExcess();

        wordsThatCanBeSelected.ForEach(data => Destroy(data.gameObject));
        wordsThatCanBeSelected.Clear();
        wordsThatCanBeSelected.TrimExcess();

        craftingWordsCanvas.gameObject.SetActive(false);
    }

    public void ExitState()
    {
        enabled = false;
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void OnInputActionPerformedClickOnThings(InputAction.CallbackContext context)
    {
        if (!IsWordClickedOn(out WordBehaviour foundWord)) return;

        Debug.Log("Found WordData: " + foundWord.wordData.presentedWord);
        if (toBeCrafted.Contains(foundWord))
        {
            toBeCrafted.Remove(foundWord);
            Destroy(foundWord.gameObject);
        }
        else
        {
            if (toBeCrafted.Count >= amountOfNounsThatCanBeCraftedInTotal)
            {
                Destroy(toBeCrafted[0].gameObject);
                toBeCrafted.RemoveAt(0);
            }

            toBeCrafted.Add(Instantiate(foundWord, craftingWordsTablePanel));
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(craftingWordsTablePanel);
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

    public void CloseCrafting()
    {
        ActorControlTypeStateMachine.PopStateToPrevious();
    }

    public void TryCraftWords()
    {
        CraftableManager instance = CraftableManager.Instance();

        List<NounData> nouns = new();
        string message;
        toBeCrafted.ForEach(behaviour => nouns.Add(behaviour.wordData as NounData));
        if (instance.TryCraftWords(nouns, out VerbData verb) && ActorManager.TryAddWordToWordsCollected(verb))
            message = $"You learned the word '{verb.presentedWord}'!\n\nCongratulations!";
        else
            message = "You couldn't craft a new word";
        ActorControlTypeStateMachine.PushStateToPopUpNotif(message);
    }
}
