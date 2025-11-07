using UnityEngine;

public class GateBehaviour : MonoBehaviour, IRewardable
{
    private static GateBehaviour instance;

    private void Awake()
    {
        if (instance != null && this != instance)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    public void InvokeReward()
    {
        if (instance == null) return;
        Debug.Log(instance.gameObject.scene.name);
        Destroy(instance.gameObject);
    }
}
