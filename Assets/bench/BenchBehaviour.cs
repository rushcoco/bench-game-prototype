using System.Collections;
using UnityEngine;

public class BenchBehaviour : MonoBehaviour, IInteractable, ISittable
{
    [SerializeField] private Transform placeToPlaySitAnimation;
    [SerializeField] private Transform placeWherePlayerWillStandUpTo;

    public void Interact()
    {
        ActorControlTypeStateMachine.PushStateToCrafting();
        ActorManager.PlaySittingOnSittableAnimation(this);
    }

    public void SwitchToActionState()
    {
    }

    public void OnSitStart(Transform actor)
    {
        //actor.position = placeToPlaySitAnimation.position;
        StartCoroutine(MovingToStartSitPosition(actor));
    }

    public void OnSitEnd(Transform actor)
    {
        //actor.position = placeWherePlayerWillStandUpTo.position;
    }

    private IEnumerator MovingToStartSitPosition(Transform actorTransform)
    {
        Vector3 initialPos = actorTransform.position;
        float secondsLeft = 0f;
        float totalSeconds = 0.35f;
        while (secondsLeft < totalSeconds)
        {
            actorTransform.position = Vector3.Lerp(initialPos, placeWherePlayerWillStandUpTo.position,
                secondsLeft / totalSeconds);
            secondsLeft += Time.deltaTime;
            yield return null;
        }
    }
}
