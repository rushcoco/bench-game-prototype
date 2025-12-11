using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private Scene benchGame;
    [SerializeField] private int thisSceneID;
    [SerializeField] private string sceneName;

    private void Awake()
    {
    }

    public void OnStartGame()
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadSceneAsync(sceneName);
        else
            Debug.LogError("No scene configured");
    }
}
