using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Text Construction")] [SerializeField]
    private int amountOfLettersToBeAddedWhenNormal;

    [SerializeField] private int amountOfLettersToBeAddedWhenSpeedUp;
    [SerializeField] private float timePassPerLetterInDialogue;

    [Header("Diegetic UI Behaviour")] [SerializeField]
    private float offsetVerticalPlayerUIHighlight;

    [SerializeField] private float offsetHorizontalPlayerUIHighlight;
    [SerializeField] private float lerpUIHighlightByThisValue;

    [Header("Global Volume Configurations")] [SerializeField]
    private Volume globalVolumeProfile;

    [Header("Diegetic UI Game Objects")] [SerializeField]
    private GameObject highlightInspect;

    [SerializeField] private GameObject highlightInteract;
    [SerializeField] private GameObject highlightListen;
    [SerializeField] private GameObject highlightTalk;
    [SerializeField] private GameObject uiHighlighter;

    [Header("Text Mesh Pro UI Prefabs")] [SerializeField]
    private TextMeshProUGUI tmpChitChat;

    [SerializeField] private TextMeshProUGUI tmpPrompt;
    [SerializeField] private TextMeshProUGUI tmpNotificationMessage;

    private Coroutine coroutineTextForTMP;
    private Coroutine coroutineUIHighlighter;

    private bool isUsingSpeedUpForTMP;
    private LensDistortion lensDistortion;

    private Camera mainCamera;
    private Vector2 referenceScaler;
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

        isUsingSpeedUpForTMP = false;
        TryResolveLensDistortion();
    }

    private void OnEnable()
    {
        mainCamera = Camera.main;
        referenceScaler = Vector2.zero;
        referenceScalerUI = uiHighlighter.GetComponent<CanvasScaler>().referenceResolution;
        coroutineTextForTMP = null;
        coroutineUIHighlighter = null;

        TryResolveLensDistortion();
    }

    private void OnDisable()
    {
        mainCamera = null;
        referenceScaler = Vector2.zero;
        coroutineTextForTMP = null;
        coroutineUIHighlighter = null;
    }

    private void TryResolveLensDistortion()
    {
        if (globalVolumeProfile != null && globalVolumeProfile.profile != null)
            globalVolumeProfile.profile.TryGet(out lensDistortion);
    }

    public void ShowUIElementInspect()
    {
        ShowUIElement(highlightInspect, highlightInspectRect);
    }

    public void ShowUIElementInteract()
    {
        ShowUIElement(highlightInteract, highlightInteractRect);
    }

    public void ShowUIElementListenOrTalk()
    {
        ShowUIElement(highlightListen, highlightListenRect);
    }

    private void ShowUIElement(GameObject highlight, RectTransform rectTransform)
    {
        highlight.SetActive(true);
        referenceScaler = mainCamera.pixelRect.size;

        if (coroutineUIHighlighter != null)
            StopCoroutine(coroutineUIHighlighter);

        coroutineUIHighlighter = instance.StartCoroutine(ShowUIHighlighter(rectTransform));
    }

    public void HideUIElementInspect()
    {
        HideUIElement(highlightInspect);
    }

    public void HideUIElementInteract()
    {
        HideUIElement(highlightInteract);
    }

    public void HideUIElementListenOrTalk()
    {
        HideUIElement(highlightListen);
    }

    private void HideUIElement(GameObject highlight)
    {
        if (coroutineUIHighlighter != null)
            StopCoroutine(coroutineUIHighlighter);

        highlight.SetActive(false);
    }

    public static void InsertTextForTMP(string followingText)
    {
        if (instance.coroutineTextForTMP != null)
            instance.StopCoroutine(instance.coroutineTextForTMP);

        instance.coroutineTextForTMP = instance.StartCoroutine(instance.ShowText(followingText, instance.tmpChitChat));
    }

    public static void InsertPromptTextForTMP(string followingText)
    {
        if (instance.coroutineTextForTMP != null)
            instance.StopCoroutine(instance.coroutineTextForTMP);

        instance.coroutineTextForTMP =
            instance.StartCoroutine(instance.ShowText(followingText, instance.tmpPrompt));
    }

    public static void InsertNotificationMessagePopText(string followingText)
    {
        if (instance.coroutineTextForTMP != null)
            instance.StopCoroutine(instance.coroutineTextForTMP);

        instance.coroutineTextForTMP =
            instance.StartCoroutine(instance.ShowText(followingText, instance.tmpNotificationMessage));
    }

    public static bool SpeedUpDialog()
    {
        instance.isUsingSpeedUpForTMP = true;
        Debug.Log("Is Speeding Up");

        return !instance.coroutineTextForTMP.IsUnityNull();
    }

    public static void SlowDownDialog()
    {
        instance.isUsingSpeedUpForTMP = false;
        Debug.Log("Is Slowing Down");
    }

    public void EditUIHighlighters(bool value)
    {
        if (!uiHighlighter.IsUnityNull())
            uiHighlighter.SetActive(value);
    }

    private Vector2 ApplyDistortion(float x, float y)
    {
        return ApplyDistortion(new Vector2(x, y));
    }

    private Vector2 ApplyDistortion(Vector2 uv)
    {
        if (!lensDistortion || !lensDistortion.active) return uv;

        float intensity = lensDistortion.intensity.value;
        float multiplierX = lensDistortion.xMultiplier.value;
        float multiplierY = lensDistortion.yMultiplier.value;
        Vector2 center = lensDistortion.center.value;
        float scale = lensDistortion.scale.value;

        Vector2 uvShiftedCenter = uv - center;

        Vector2 p = new(uvShiftedCenter.x * 2, uvShiftedCenter.y * 2);

        p.x *= scale * Mathf.Max(1e-4f, multiplierX);
        p.y *= scale * Mathf.Max(1e-4f, multiplierY);

        float r2 = p.x * p.x + p.y * p.y;
        float k = intensity * 0.75f;

        float factor = 1f + k * r2;
        Vector2 dp = p * factor;

        dp.x /= scale * Mathf.Max(1e-4f, multiplierX);
        dp.y /= scale * Mathf.Max(1e-4f, multiplierY);

        Vector2 outUv = new Vector2(dp.x * 0.5f, dp.y * 0.5f) + center;

        return outUv;
    }

    private IEnumerator ShowText(string followingText, TextMeshProUGUI tmpGUI)
    {
        tmpGUI.text = "";
        int amountChar = followingText.Length;
        int currChar = 0;
        float dtCurrently = 0f;

        while (currChar < amountChar)
        {
            dtCurrently += Time.deltaTime;

            if (dtCurrently >= timePassPerLetterInDialogue)
            {
                int amountOfLettersToBeAdded = isUsingSpeedUpForTMP
                    ? amountOfLettersToBeAddedWhenSpeedUp
                    : amountOfLettersToBeAddedWhenNormal;
                for (int i = 0; i < amountOfLettersToBeAdded && amountChar > currChar; i++)
                {
                    tmpGUI.text += followingText[currChar];
                    currChar++;
                }

                dtCurrently = 0f;
            }

            yield return null;
        }

        if (!coroutineTextForTMP.IsUnityNull())
            coroutineTextForTMP = null;
    }

    private IEnumerator ShowUIHighlighter(RectTransform highlightElement)
    {
        Vector3 worldToViewport =
            mainCamera.WorldToViewportPoint(ActorManager.GetCameraTargetPosition());
        Vector2 viewportUIElement = ApplyDistortion(worldToViewport.x, worldToViewport.y);


        viewportUIElement.y += offsetVerticalPlayerUIHighlight / referenceScalerUI.y;

        while (highlightElement.gameObject.activeSelf)
        {
            worldToViewport = mainCamera.WorldToViewportPoint(ActorManager.GetCameraTargetPosition());
            Vector2 viewportUIElementDesired = ApplyDistortion(worldToViewport.x, worldToViewport.y);

            viewportUIElementDesired.y += offsetVerticalPlayerUIHighlight / referenceScalerUI.y;

            Vector2 viewPortUIElementReal = Vector2.Lerp(viewportUIElement, viewportUIElementDesired,
                lerpUIHighlightByThisValue);

            highlightElement.position = viewPortUIElementReal * referenceScaler;

            viewportUIElement = viewPortUIElementReal;

            yield return null;
        }

        coroutineUIHighlighter = null;
    }
}
