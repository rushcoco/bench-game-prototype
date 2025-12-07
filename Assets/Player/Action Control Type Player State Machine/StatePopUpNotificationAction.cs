using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePopUpNotificationAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private RectTransform popUpCanvas;
    [SerializeField] private InputActionReference speedUpText;
    private readonly Queue<string> notificationMessage = new();

    private void OnEnable()
    {
        popUpCanvas.gameObject.SetActive(true);
        SetMessageStringToUIElement();
        
        speedUpText.action.Enable();

        speedUpText.action.started += OnInputActionStartedSpeedUpText;
        speedUpText.action.canceled += OnInputActionCanceledSpeedUpText;

        ActorControlTypeStateMachine.SetCursorModes(true, CursorLockMode.None);
    }

    private void OnDisable()
    {
        ActorControlTypeStateMachine.SetCursorModes(false, CursorLockMode.Locked);

        speedUpText.action.started -= OnInputActionStartedSpeedUpText;
        speedUpText.action.canceled -= OnInputActionCanceledSpeedUpText;
        
        speedUpText.action.Disable();

        popUpCanvas.gameObject.SetActive(false);
    }

    public void ExitState()
    {
        enabled = false;
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void SetMessageStringToUIElement()
    {
        UIController.InsertNotificationMessagePopText(notificationMessage.Dequeue());
    }

    public void OnContinue()
    {
        if (notificationMessage.Count > 0)
            SetMessageStringToUIElement();
        else
            ActorControlTypeStateMachine.PopStateToPrevious();
    }

    public void AddNotificationMessage(string message)
    {
        // Add Notification Message to a list or stack or smth
        notificationMessage.Enqueue(message);
    }
    
    private void OnInputActionStartedSpeedUpText(InputAction.CallbackContext context)
    {
        UIController.SpeedUpDialog();
    }

    private void OnInputActionCanceledSpeedUpText(InputAction.CallbackContext context)
    {
        UIController.SlowDownDialog();
    }
}
