using UnityEngine;
using UnityEngine.InputSystem;

public class StateListeningAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputToNextOrSpeedUp;
    [SerializeField] private InputActionReference inputImmediateChitChatCompletion;
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

        inputImmediateChitChatCompletion.action.Enable();
        inputToNextOrSpeedUp.action.Enable();

        inputImmediateChitChatCompletion.action.performed += OnInputActionPerformedToNextChitChat;
        inputToNextOrSpeedUp.action.started += OnInputActionStartedSpeedUpText;
        inputToNextOrSpeedUp.action.canceled += OnInputActionCanceledSpeedUpText;

        ActorManager.OnEnterMoveCameraToCaptureActorWithConversable(currentConversable);
    }

    private void OnDisable()
    {
        inputImmediateChitChatCompletion.action.performed -= OnInputActionPerformedToNextChitChat;
        inputToNextOrSpeedUp.action.started -= OnInputActionStartedSpeedUpText;
        inputToNextOrSpeedUp.action.canceled -= OnInputActionCanceledSpeedUpText;

        inputImmediateChitChatCompletion.action.Disable();
        inputToNextOrSpeedUp.action.Disable();

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

    private void OnInputActionPerformedToNextChitChat(InputAction.CallbackContext context)
    {
        // Complete the current Sentence
    }

    private void OnInputActionStartedSpeedUpText(InputAction.CallbackContext context)
    {
        Debug.Log(UIController.SpeedUpDialog());
        if (UIController.SpeedUpDialog()) return;
        
        if (currentConversable.NextChitChat()) return;
        ActorControlTypeStateMachine.PopStateToPrevious();
    }

    private void OnInputActionCanceledSpeedUpText(InputAction.CallbackContext context)
    {
        UIController.SlowDownDialog();
    }
}
