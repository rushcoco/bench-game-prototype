using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private float timePassPerLetterInDialogueInMilliSeconds;
    [SerializeField] private float offsetVerticalPlayerUIHighlight;
    [SerializeField] private float offsetHorizontalPlayerUIHighlight;
    [SerializeField] private float lerpUIHighlightByThisValue;
    [SerializeField] private Material retroPostProcessing;
    [SerializeField] private GameObject highlightInspect;
    [SerializeField] private GameObject highlightInteract;
    [SerializeField] private GameObject highlightListen;
    [SerializeField] private GameObject highlightTalk;
    [SerializeField] private TextMeshProUGUI tmpChitChat;
    [SerializeField] private TextMeshProUGUI tmpPrompt;
    [SerializeField] private TextMeshProUGUI tmpNotificationMessage;
    [SerializeField] private GameObject uiHighlighter;
    private Coroutine coroutineNotificationPopUpMessage;
    private Coroutine coroutinePromptTextForTMP;
    private Coroutine coroutineTextForTMP;
    private Camera mainCamera;
    private Vector2 referenceScalerUI;
    private RectTransform highlightInspectRect => highlightInspect.transform as RectTransform;
    private RectTransform highlightInteractRect => highlightInteract.transform as RectTransform;
    private RectTransform highlightListenRect => highlightListen.transform as RectTransform;
    private RectTransform uiHighlightRect => uiHighlighter.transform as RectTransform;

    public static UIController instance { get; private set; }

    private void Awake()
    {
        if (!instance.IsUnityNull() && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void OnEnable()
    {
        mainCamera = Camera.main;
        referenceScalerUI = uiHighlighter.GetComponent<CanvasScaler>().referenceResolution;
        coroutineTextForTMP = null;
        coroutinePromptTextForTMP = null;
        coroutineNotificationPopUpMessage = null;
    }

    private void OnDisable()
    {
        mainCamera = null;
        referenceScalerUI = Vector2.zero;
        coroutineTextForTMP = null;
        coroutinePromptTextForTMP = null;
        coroutineNotificationPopUpMessage = null;
    }

    public void ShowUIElementInspect()
    {
        highlightInspect.SetActive(true);
        instance.StartCoroutine(ShowUIHighlighter(highlightInspectRect));
    }

    public void ShowUIElementInteract()
    {
        highlightInteract.SetActive(true);
        instance.StartCoroutine(ShowUIHighlighter(highlightInteractRect));
    }

    public void ShowUIElementListenOrTalk()
    {
        highlightListen.SetActive(true);
        instance.StartCoroutine(ShowUIHighlighter(highlightListenRect));
    }

    private void ShowUIElement()
    {
    }

    public void HideUIElementInspect()
    {
        instance.StopCoroutine(nameof(ShowUIHighlighter));
        highlightInspect.SetActive(false);
    }

    public void HideUIElementInteract()
    {
        instance.StopCoroutine(nameof(ShowUIHighlighter));
        highlightInteract.SetActive(false);
    }

    public void HideUIElementListenOrTalk()
    {
        instance.StopCoroutine(nameof(ShowUIHighlighter));
        highlightListen.SetActive(false);
    }

    public static void InsertTextForTMP(string followingText)
    {
        if (instance.coroutineTextForTMP != null)
            instance.StopCoroutine(instance.coroutineTextForTMP);

        instance.coroutineTextForTMP = instance.StartCoroutine(instance.ShowText(followingText, instance.tmpChitChat));
    }

    public static void InsertPromptTextForTMP(string followingText)
    {
        if (instance.coroutinePromptTextForTMP != null)
            instance.StopCoroutine(instance.coroutinePromptTextForTMP);

        instance.coroutinePromptTextForTMP =
            instance.StartCoroutine(instance.ShowText(followingText, instance.tmpPrompt));
    }

    public static void InsertNotificationMessagePopText(string followingText)
    {
        if (instance.coroutineNotificationPopUpMessage != null)
            instance.StopCoroutine(instance.coroutineNotificationPopUpMessage);

        instance.coroutineNotificationPopUpMessage =
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

    private IEnumerator ShowUIHighlighter(RectTransform highlightElement)
    {
        Vector2 uiPositionBefore =
            mainCamera.WorldToViewportPoint(ActorManager.GetCameraTargetPosition()) * referenceScalerUI;
        while (true)
        {
            Vector2 uiPositionDesired =
                mainCamera.WorldToViewportPoint(ActorManager.GetCameraTargetPosition()) * referenceScalerUI;

            Vector2 uiPositionDefinitive = Vector2.Lerp(uiPositionBefore, uiPositionDesired,
                lerpUIHighlightByThisValue);

            highlightElement.position =
                new Vector3(uiPositionDefinitive.x ,uiPositionDefinitive.y + offsetVerticalPlayerUIHighlight, 0f);
            yield return null;
        }
    }
}
