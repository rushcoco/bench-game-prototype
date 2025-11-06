using UnityEngine;

public class SimpleNotificaterCallerBehaviour : MonoBehaviour, IInspectable
{
    [SerializeField] private string message;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Inspect()
    {
        // Show 2 Pop Ups
        // 
        ActorControlTypeStateMachine.PushStateToPopUpNotif(message);
    }
}
