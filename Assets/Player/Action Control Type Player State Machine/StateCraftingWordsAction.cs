using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateCraftingWordsAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputClickOnThings;
    [SerializeField] private InputActionReference inputCursorPosition;
    [SerializeField] private RectTransform craftingWordsCanvas;
    [SerializeField] private RectTransform craftWordsBoxPanel;
    [SerializeField] private RectTransform craftWordsSelectorPanel;

    [SerializeField] private GameObject emptyUIGameObject;
    private List<WordBehaviour> wordsThatCanBeSelected;
    private List<WordBehaviour> wordsReadyForCrafting;

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

        inputClickOnThings.action.Enable();
        inputCursorPosition.action.Enable();

        inputClickOnThings.action.performed += OnInputActionPerformedClickOnThings;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        wordsThatCanBeSelected = new List<WordBehaviour>();
        wordsReadyForCrafting = new List<WordBehaviour>();

        foreach (NounData nounData in ActorManager.GetAllNounsPlayerHasCollected())
        {
            WordBehaviour currEmptyWord = Instantiate(emptyUIGameObject, craftWordsSelectorPanel).AddComponent<WordBehaviour>();
            currEmptyWord.SetWord(nounData);
            currEmptyWord.GetComponent<TextMeshProUGUI>().text = currEmptyWord.wordData.presentedWord;
            
            wordsThatCanBeSelected.Add(currEmptyWord);
        }
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inputClickOnThings.action.performed -= OnInputActionPerformedClickOnThings;

        inputClickOnThings.action.Disable();
        inputCursorPosition.action.Disable();

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
    }

    public void CloseCrafting()
    {
        ActorControlTypeStateMachine.PopStateToPrevious();
    }
}
