using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorControlTypeStateMachine : MonoBehaviour
{
    private static ActorControlTypeStateMachine instance;
    [SerializeField] private StateOverworldMovement overworldMovement;
    [SerializeField] private StateListeningAction listeningAction;
    [SerializeField] private StateTalkingAction talkingAction;
    [SerializeField] private StateCraftingWordsAction craftingWordsAction;
    [SerializeField] private StatePopUpNotificationAction popUpNotificationAction;

    public bool actorCurserVisible;
    public CursorLockMode actorCursorLock;
    private readonly Stack<IControlTypeState> stateStack = new();
    private IControlTypeState currentState;

    private void Awake()
    {
        if (!instance.IsUnityNull() && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // overworldMovement.enabled = false;
        listeningAction.enabled = false;
        talkingAction.enabled = false;
        craftingWordsAction.enabled = false;
        popUpNotificationAction.enabled = false;

        SetCursorModes(false, CursorLockMode.Locked);
        PushState(overworldMovement);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ApplyCursorSettings()
    {
        Cursor.visible = actorCurserVisible;
        Cursor.lockState = actorCursorLock;
    }

    private void SetState(IControlTypeState newState)
    {
        if (currentState == newState) return;
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
        ApplyCursorSettings();
    }

    private void PushState(IControlTypeState newState)
    {
        if (currentState != null)
            if (newState != currentState)
                stateStack.Push(currentState);
        SetState(newState);
    }

    private void PopState()
    {
        SetState(stateStack.Count > 0 ? stateStack.Pop() : overworldMovement);
    }

    public static void PushStateToCrafting()
    {
        instance.PushState(instance.craftingWordsAction);
    }

    public static void PopStateToPrevious()
    {
        instance.PopState();
    }

    public static void PushStateToPopUpNotif(string notifMessage)
    {
        instance.popUpNotificationAction.AddNotificationMessage(notifMessage);
        instance.PushState(instance.popUpNotificationAction);
    }

    public static void PushStateToPopUpNotif(IReadOnlyCollection<string> notifMessage)
    {
        foreach (string s in notifMessage) instance.popUpNotificationAction.AddNotificationMessage(s);
        instance.PushState(instance.popUpNotificationAction);
    }

    public static void PushStateToListening(IConversable currentConversable)
    {
        instance.listeningAction.SetIConversable(currentConversable);
        instance.PushState(instance.listeningAction);
    }

    public static void PushStateToTalking(IConversable currentConversable)
    {
        instance.talkingAction.SetIConversable(currentConversable);
        instance.PushState(instance.talkingAction);
    }

    public static void PushStateToNoActorControl(IControlTypeState controlTypeState)
    {
        instance.PushState(controlTypeState);
    }

    public static void ChangeStateToListening(IConversable currentConversable)
    {
        PopStateToPrevious();
        PushStateToListening(currentConversable);
    }

    public static void SetCursorModes(bool cursorVisible, CursorLockMode cursorLockMode)
    {
        instance.actorCurserVisible = cursorVisible;
        instance.actorCursorLock = cursorLockMode;
    }
}
