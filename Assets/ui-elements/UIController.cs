using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject highlightInspect;
    [SerializeField] private GameObject highlightInteract;
    [SerializeField] private GameObject highlightListen;
    [SerializeField] private GameObject highlightTalk;
    [SerializeField] private TextMeshProUGUI tmpChitChat;
    [SerializeField] private TextMeshProUGUI tmpPrompt;
    [SerializeField] private TextMeshProUGUI tmpNotificationMessage;
    public RectTransform uiHighlighter;
    public static UIController instance { get; private set; }

    private void Awake()
    {
        if (!instance.IsUnityNull() && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ShowUIElementInspect()
    {
        highlightInspect.SetActive(true);
    }

    public void ShowUIElementInteract()
    {
        highlightInteract.SetActive(true);
    }

    public void ShowUIElementListen()
    {
        highlightListen.SetActive(true);
    }

    public void ShowUIElementTalk()
    {
        highlightTalk.SetActive(true);
    }

    private void ShowUIElement()
    {
    }

    public void HideUIElementInspect()
    {
        highlightInspect.SetActive(false);
    }

    public void HideUIElementInteract()
    {
        highlightInteract.SetActive(false);
    }

    public void HideUIElementListen()
    {
        highlightListen.SetActive(false);
    }

    public void HideUIElementTalk()
    {
        highlightTalk.SetActive(false);
    }

    public void ShowSpeechBubble()
    {
    }

    public void HideSpeechBubble()
    {
    }

    public static void InsertTextForTMP(string followingText)
    {
        instance.tmpChitChat.text = followingText;
    }

    public static void InsertPromptTextForTMP(string followingText)
    {
        instance.tmpPrompt.text = followingText;
    }

    public static void InsertNotificationMessagePopText(string followingText)
    {
        instance.tmpNotificationMessage.text = followingText;
    }
}
