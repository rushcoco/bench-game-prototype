using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private float timePassPerLetterInDialogueInMilliSeconds;
    [SerializeField] private GameObject highlightInspect;
    [SerializeField] private GameObject highlightInteract;
    [SerializeField] private GameObject highlightListen;
    [SerializeField] private GameObject highlightTalk;
    [SerializeField] private TextMeshProUGUI tmpChitChat;
    [SerializeField] private TextMeshProUGUI tmpPrompt;
    [SerializeField] private TextMeshProUGUI tmpNotificationMessage;
    [SerializeField] private GameObject uiHighlighter;
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
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShowText(followingText, instance.tmpChitChat));
    }

    public static void InsertPromptTextForTMP(string followingText)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShowText(followingText, instance.tmpPrompt));
    }

    public static void InsertNotificationMessagePopText(string followingText)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShowText(followingText, instance.tmpNotificationMessage));
    }

    public void EditUIHighlighters(bool value)
    {
        if (!uiHighlighter.IsUnityNull())
            uiHighlighter.SetActive(value);
    }

    private IEnumerator ShowText(string followingText, TextMeshProUGUI tmpGUI)
    {
        tmpGUI.text = "";
        int amountChar = followingText.Length;
        int currChar = 0;
        float timePassed = 0f;
        float timePassTotal = timePassPerLetterInDialogueInMilliSeconds * 0.001f;

        while (currChar < amountChar)
            if (timePassed >= timePassTotal)
            {
                timePassed = 0f;
                tmpGUI.text += followingText[currChar];
                currChar++;
                yield return null;
            }
            else
            {
                timePassed += Time.deltaTime;
            }
    }
}
