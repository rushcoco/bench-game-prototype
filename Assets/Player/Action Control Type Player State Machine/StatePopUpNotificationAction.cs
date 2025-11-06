using UnityEngine;

public class StatePopUpNotificationAction : MonoBehaviour, IControlTypeState
{
    [SerializeField] private RectTransform popUpCanvas;
    public string notificationMessage;

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
        UIController.InsertNotificationMessagePopText(notificationMessage);
    }

    public void OnContinue()
    {
        ActorControlTypeStateMachine.PopStateToPrevious();
    }
}
