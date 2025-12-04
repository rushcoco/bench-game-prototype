using System.Collections;
using UnityEngine;

public class OpenDoorRewardHandler : RewardHandler
{
    [SerializeField] private Vector3 onOpenDoorTranslatesWhere;
    [SerializeField] private Vector3 onOpenDoorAxisRotatedAround;
    [SerializeField] private float angleOfDoorOpened;
    [SerializeField] private float secondsOfDoorOpening;

    protected override void HandlePuzzleSolved()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(OpenDoor());
        // Open the gardeners door aka this
    }

    private IEnumerator OpenDoor()
    {
        float secondsLeft = 0f;
        while (secondsLeft < secondsOfDoorOpening)
        {
            // transform.Translate((Vector3.forward + Vector3.right) * Time.deltaTime / secondsOfDoorOpening);
            transform.parent.Rotate(Vector3.up, angleOfDoorOpened * Time.deltaTime / secondsOfDoorOpening);

            secondsLeft += Time.deltaTime;
            yield return null;
        }
    }
}
