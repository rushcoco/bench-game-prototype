using System.Collections;
using UnityEngine;

public class OpenDoorRewardHandler : RewardHandler
{
    [SerializeField] private Vector3 onOpenDoorTranslatesWhere;
    [SerializeField] private Vector3 onOpenDoorAxisRotatedAround;
    [SerializeField] private float angleOfDoorOpened;
    [SerializeField] private float secondsOfDoorOpening;

    private void Start()
    {
    }

    private void Update()
    {
    }

    protected override void HandlePuzzleSolved()
    {
        StartCoroutine(OpenDoor(transform.position));
        // Open the gardeners door aka this
    }

    private IEnumerator OpenDoor(Vector3 position)
    {
        float secondsLeft = 0f;
        while (secondsLeft < secondsOfDoorOpening)
        {
            transform.Translate((Vector3.forward + Vector3.right) * Time.deltaTime / secondsOfDoorOpening);
            transform.Rotate(Vector3.up, angleOfDoorOpened * Time.deltaTime / secondsOfDoorOpening);

            secondsLeft += Time.deltaTime;
            yield return null;
        }
    }
}
