using System.Collections.Generic;
using UnityEngine;

public class StatePopUpNotificationAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private RectTransform popUpCanvas;
    private readonly Queue<string> notificationMessage = new();

    private void OnEnable()
    {
        popUpCanvas.gameObject.SetActive(true);
        SetMessageStringToUIElement();

        ActorControlTypeStateMachine.SetCursorModes(true, CursorLockMode.None);
    }

    private void OnDisable()
    {
        ActorControlTypeStateMachine.SetCursorModes(false, CursorLockMode.Locked);

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
}
