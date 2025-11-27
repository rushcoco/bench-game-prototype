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

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        canvasDialogBox.gameObject.SetActive(true);

        inputToNextChitChat.action.Enable();

        inputToNextChitChat.action.started += OnInputActionStartedToNextChitChat;


        ActorManager.OnEnterMoveCameraToCaptureActorWithConversable(currentConversable);
    }

    private void OnDisable()
    {
        inputToNextChitChat.action.started -= OnInputActionStartedToNextChitChat;

        inputToNextChitChat.action.Disable();

        canvasDialogBox.gameObject.SetActive(false);

        ActorManager.OnExitMoveCameraToCaptureActorWithConversable(currentConversable);
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
        ActorControlTypeStateMachine.PopStateToPrevious();
    }

    private void OnInputActionCanceledSpeedUpText(InputAction.CallbackContext context)
    {
    }
}
