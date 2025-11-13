using System.Collections.Generic;
using UnityEngine;

public class SimpleNotificaterCallerBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private List<string> message;

    public void Inspect()
    {
        ActorControlTypeStateMachine.PushStateToPopUpNotif(message);
    }
}
