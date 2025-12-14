using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BenchBehaviour : MonoBehaviour, IInteractable, ISittable
{
    [SerializeField] private Transform placeToPlaySitAnimation;
    [SerializeField] private Transform placeWherePlayerWillStandUpTo;
    [SerializeField] private List<Renderer> interactThisMesh;

    public void Interact()
    {
        ActorControlTypeStateMachine.PushStateToCrafting();
        ActorManager.PlaySittingOnSittableAnimation(this);
    }

    public void SwitchToActionState()
    {
    }

    public void ShowHighlight(Material material)
    {
        foreach (Renderer inspectMesh in interactThisMesh)
        {
            int length = inspectMesh.materials.Length;
            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = 0; i < length; i++) materials.Insert(i * 2 + 1, material);
            inspectMesh.materials = materials.ToArray();

            materials.Clear();
            materials.TrimExcess();
        }
    }

    public void HideHighlight(Material material)
    {
        foreach (Renderer inspectMesh in interactThisMesh)
        {
            int length = Mathf.FloorToInt(inspectMesh.materials.Length * 0.5f);

            List<Material> materials = inspectMesh.materials.ToList();

            for (int i = length; i > 0; i--)
                materials.RemoveAt(i * 2 - 1);

            inspectMesh.materials = materials.ToArray();
        }
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
