using UnityEngine;

public class GateRewardHandler : RewardHandler
{
    protected override void HandlePuzzleSolved()
    {
        Debug.Log(gameObject.scene.name);
        Destroy(gameObject);
    }
}
