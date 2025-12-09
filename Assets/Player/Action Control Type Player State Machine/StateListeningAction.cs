using UnityEngine;
using UnityEngine.InputSystem;

public class StateListeningAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private InputActionReference inputToNextChitChat;
    [SerializeField] private InputActionReference inputToSpeedUpDialog;

    [SerializeField] private RectTransform canvasDialogBox;

    private IConversable currentConversable;
    private bool hasPressedInputToNextChitChat;

    private void OnEnable()
    {
        canvasDialogBox.gameObject.SetActive(true);

        inputToNextChitChat.action.Enable();
        inputToSpeedUpDialog.action.Enable();

        inputToNextChitChat.action.performed += OnInputActionPerformedToNextChitChat;
        inputToSpeedUpDialog.action.started += OnInputActionStartedSpeedUpText;
        inputToSpeedUpDialog.action.canceled += OnInputActionCanceledSpeedUpText;

        ActorManager.OnEnterMoveCameraToCaptureActorWithConversable(currentConversable);
    }

    private void OnDisable()
    {
        inputToNextChitChat.action.performed -= OnInputActionPerformedToNextChitChat;
        inputToSpeedUpDialog.action.started -= OnInputActionStartedSpeedUpText;
        inputToSpeedUpDialog.action.canceled -= OnInputActionCanceledSpeedUpText;

        inputToNextChitChat.action.Disable();
        inputToSpeedUpDialog.action.Disable();

        UIController.SlowDownDialog();

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
        if (currentConversable.NextChitChat()) return;
        ActorControlTypeStateMachine.PopStateToPrevious();
    }

    private void OnInputActionStartedSpeedUpText(InputAction.CallbackContext context)
    {
        Debug.Log(UIController.SpeedUpDialog());
        if (UIController.SpeedUpDialog()) return;
    }

    private void OnInputActionCanceledSpeedUpText(InputAction.CallbackContext context)
    {
        UIController.SlowDownDialog();
    }
}
