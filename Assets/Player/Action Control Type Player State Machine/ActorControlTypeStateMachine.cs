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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        overworldMovement.enabled = false;
        listeningAction.enabled = false;
        talkingAction.enabled = false;
        craftingWordsAction.enabled = false;

        ChangeStateToOverworldMovement();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SetState(IControlTypeState newState)
    {
        if (currentState == newState) return;
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
        Debug.Log("current State: " + currentState.GetType());
    }

    private void PushState(IControlTypeState newState)
    {
        if (currentState != null)
            stateStack.Push(currentState);
        SetState(newState);
    }

    private void PopState()
    {
        SetState(stateStack.Count > 0 ? stateStack.Pop() : overworldMovement);
    }

    public static void ChangeStateToListening(IConversable currentConversable)
    {
        instance.listeningAction.SetIConversable(currentConversable);
        instance.SetState(instance.listeningAction);
    }

    public static void ChangeStateToOverworldMovement()
    {
        instance.SetState(instance.overworldMovement);
    }

    public static void ChangeStateToTalking(IConversable currentConversable)
    {
        instance.talkingAction.SetIConversable(currentConversable);
        instance.SetState(instance.talkingAction);
    }

    public static void PushStateToCrafting()
    {
        instance.PushState(instance.craftingWordsAction);
    }

    public static void PopStateToPrevious()
    {
        instance.PopState();
    }
}
