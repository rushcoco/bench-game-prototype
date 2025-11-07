using UnityEngine;

public class GateBehaviour : MonoBehaviour, IRewardable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    private void UnlockGate()
    {
        Destroy(gameObject);
    }

    public void InvokeReward()
    {
        UnlockGate();
    }
}
