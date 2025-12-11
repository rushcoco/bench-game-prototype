using System.Collections;
using UnityEngine;

public class GateRewardHandler : RewardHandler, IControlTypeState
{
    private static readonly int NoiseStrength = Shader.PropertyToID("_NoiseStrength");
    private static readonly int NoiseActivated = Shader.PropertyToID("_NoiseActivated");
    private static readonly int EdgeWidth = Shader.PropertyToID("_EdgeWidth");
    private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");

    [Header("Gate Duration Seconds Configuration")] [SerializeField]
    private float secondsUntilGateIsGone;

    [SerializeField] private float waitForSecondsAtDestroyedGate;
    [SerializeField] private float waitForSecondsUntilCameraArrivesAtGate;

    [Header("Edge Simple Noise Configuration")] [SerializeField]
    private float edgeWidth;

    [SerializeField] private Color edgeColor;

    [Header("Component References")] [SerializeField]
    private Renderer gateMeshRenderer;

    [SerializeField] private Transform targetForCameraFocus;

    private bool isActive;
    private bool isFirstTime;

    public void ExitState()
    {
        isActive = false;
        isFirstTime = false;
        ActorManager.ResetCameraToOriginalPosition();
    }

    public void EnterState()
    {
        isActive = true;
        isFirstTime = true;
        ActorManager.MoveCameraToAFocus(targetForCameraFocus);
    }

    protected override void HandlePuzzleSolved()
    {
        ActorControlTypeStateMachine.PushStateToNoActorControl(this);
        Debug.Log(gameObject.scene.name);

        StartCoroutine(DestroyDoorSlowly());
    }

    private IEnumerator DestroyDoorSlowly()
    {
        float secondsLeft = 0f;
        float noiseStrength = 0f;
        float calcSecondsWithMultiplier = 1f / secondsUntilGateIsGone;

        Material[] gateMaterials = gateMeshRenderer.materials;

        foreach (Material material in gateMaterials)
        {
            material.SetInt(NoiseActivated, 1);
            material.SetColor(EdgeColor, edgeColor);
            material.SetFloat(EdgeWidth, edgeWidth);
        }

        while (secondsLeft < secondsUntilGateIsGone)
        {
            yield return null;

            if (!isActive) continue;

            if (isFirstTime)
            {
                yield return new WaitForSeconds(waitForSecondsUntilCameraArrivesAtGate);
                isFirstTime = !isFirstTime;
                continue;
            }

            noiseStrength += Time.deltaTime * calcSecondsWithMultiplier;

            foreach (Material material in gateMaterials) material.SetFloat(NoiseStrength, noiseStrength);

            secondsLeft += Time.deltaTime;
        }

        yield return new WaitForSeconds(waitForSecondsAtDestroyedGate);

        ActorControlTypeStateMachine.PopStateToPrevious();
        gameObject.SetActive(false);
    }
}
