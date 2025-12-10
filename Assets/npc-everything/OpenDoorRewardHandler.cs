using System.Collections;
using UnityEngine;

public class OpenDoorRewardHandler : RewardHandler, IControlTypeState
{
    [SerializeField] private Vector3 onOpenDoorTranslatesWhere;
    [SerializeField] private Vector3 onOpenDoorAxisRotatedAround;
    [SerializeField] private float angleOfDoorOpened;
    [SerializeField] private float secondsOfDoorOpening;
    [SerializeField] private float secondsOfSeeingOpenedDoor;
    private Coroutine cameraTargetLerp;

    private bool isActive;
    private Vector3 originalTargetVector;

    public void ExitState()
    {
        isActive = false;
        ActorManager.ResetCameraToOriginalPosition();
    }

    public void EnterState()
    {
        isActive = true;
        ActorManager.MoveCameraToAFocus(transform.parent);
    }

    protected override void HandlePuzzleSolved()
    {
        ActorControlTypeStateMachine.PushStateToNoActorControl(this);

        GetComponent<Collider>().enabled = false;
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor()
    {
        float secondsLeft = 0f;
        while (secondsLeft < secondsOfDoorOpening)
        {
            yield return null;

            if (!isActive) continue;

            transform.parent.Rotate(Vector3.up, angleOfDoorOpened * Time.deltaTime / secondsOfDoorOpening);

            secondsLeft += Time.deltaTime;
        }

        yield return new WaitForSeconds(secondsOfDoorOpening);

        ActorControlTypeStateMachine.PopStateToPrevious();
    }
}
