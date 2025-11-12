using UnityEngine;

public abstract class RewardHandler : MonoBehaviour
{
    public PuzzleData target;

    private void OnEnable()
    {
        target.OnPuzzleSolved += HandlePuzzleSolved;
    }

    private void OnDisable()
    {
        target.OnPuzzleSolved -= HandlePuzzleSolved;
    }

    protected abstract void HandlePuzzleSolved();
}
