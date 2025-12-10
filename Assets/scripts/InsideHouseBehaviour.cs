using System.Collections;
using UnityEngine;

public class InsideHouseBehaviour : MonoBehaviour
{
    private static readonly int SeeThroughDistance = Shader.PropertyToID("_SeeThroughDistance");
    [SerializeField] private Renderer[] meshes;
    [SerializeField] private float defaultDitherDistance;
    [SerializeField] private float insideHouseDitherDistance;
    [SerializeField] private float jumpPerSecond;
    private Coroutine isTransitioning;

    public void HideMesh()
    {
        if (isTransitioning != null)
            StopCoroutine(isTransitioning);

        isTransitioning = StartCoroutine(TransitionMaterial(meshes, insideHouseDitherDistance));
    }

    public void ShowMesh()
    {
        if (isTransitioning != null)
            StopCoroutine(isTransitioning);

        isTransitioning = StartCoroutine(TransitionMaterial(meshes, defaultDitherDistance));
    }

    private IEnumerator TransitionMaterial(Renderer[] renderers, float end)
    {
        bool isConditionFulfilled = false;
        while (!isConditionFulfilled)
        {
            isConditionFulfilled = true;

            foreach (Renderer renderer1 in renderers)
            {
                float start = renderer1.material.GetFloat(SeeThroughDistance);
                renderer1.material.SetFloat(SeeThroughDistance, start + (end - start) * Time.deltaTime * jumpPerSecond);

                if (!Mathf.Approximately(start, end))
                    isConditionFulfilled = false;
            }

            yield return null;
        }

        isTransitioning = null;
    }
}
