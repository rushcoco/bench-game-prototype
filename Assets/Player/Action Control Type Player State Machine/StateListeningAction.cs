using UnityEngine;
using UnityEngine.InputSystem;

public class StateListeningAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputToNextChitChat;
    [SerializeField] private InputActionReference inputSpeedUpText;
    [SerializeField] private RectTransform canvasDialogBox;

    private IConversable currentConversable;
    private bool hasPressedInputToNextChitChat;

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
        //Debug.Log(inputToNextChitChat.action.ReadValue<float>());
    }

    private void OnEnable()
    {
        canvasDialogBox.gameObject.SetActive(true);

        inputToNextChitChat.action.Enable();
        inputSpeedUpText.action.Enable();

        inputSpeedUpText.action.started += OnInputActionStartedSpeedUpText;
        inputSpeedUpText.action.canceled += OnInputActionCanceledSpeedUpText;
        inputToNextChitChat.action.started += OnInputActionStartedToNextChitChat;
    }

    private void OnDisable()
    {
        inputSpeedUpText.action.started -= OnInputActionStartedSpeedUpText;
        inputSpeedUpText.action.canceled -= OnInputActionCanceledSpeedUpText;
        inputToNextChitChat.action.started -= OnInputActionStartedToNextChitChat;

        inputSpeedUpText.action.Disable();
        inputToNextChitChat.action.Disable();

        canvasDialogBox.gameObject.SetActive(false);
    }

    public void ExitState()
    {
        enabled = false;
    }

    public void EnterState()
    {
        enabled = true;
    }

    public void SetIConversable(IConversable conversable)
    {
        currentConversable = conversable;
    }

    private void OnInputActionStartedToNextChitChat(InputAction.CallbackContext context)
    {
        if (currentConversable.NextChitChat()) return;
        UIController.instance.HideSpeechBubble();
        ActorControlTypeStateMachine.ChangeStateToOverworldMovement();
    }

    private void OnInputActionStartedSpeedUpText(InputAction.CallbackContext context)
    {
        currentConversable.StartSolutionChitChat();
        ActorControlTypeStateMachine.ChangeStateToListening(currentConversable);
    }

    private void OnInputActionCanceledSpeedUpText(InputAction.CallbackContext context)
    {
    }
}
