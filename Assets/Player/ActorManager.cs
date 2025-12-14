using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ActorManager : MonoBehaviour
{
    private static ActorManager instance;
    [SerializeField] private Transform actorPlayerTransform;
    [SerializeField] private List<WordData> wordsCollected;
    [SerializeField] private VerbData toHave;
    [SerializeField] private VerbData toBe;
    [SerializeField] private VerbData will;
    [SerializeField] private AudioSource soundClickOnWord;
    [SerializeField] private AudioSource soundClickOnCancelButton;
    [SerializeField] private AudioSource soundEnterCraftingMenu;
    [SerializeField] private AudioSource soundClickOnCraftButton;
    [SerializeField] private AudioSource soundOnInspect;
    [SerializeField] private Material inspectHighlightMat;
    [SerializeField] private InputActionReference inputToResetGame;
    [SerializeField] private int titleScreenSceneIndex;

    private ISittable actorIsSittingOn;
    private CameraFollowTarget cameraFollowTarget;

    private void Awake()
    {
        if (!instance.IsUnityNull() && instance != this)
            Destroy(this);
        else
            instance = this;

        if (wordsCollected == null)
            wordsCollected = new List<WordData>();

        VerbConjugator.SetAuxiliaryVerbs(toHave, toBe, will);
        cameraFollowTarget = GetComponent<CameraFollowTarget>();
    }

    private void Start()
    {
        if (wordsCollected is null)
            wordsCollected = new List<WordData>();
    }

    private void OnEnable()
    {
        inputToResetGame.action.Enable();
        inputToResetGame.action.performed += OnInputToResetEntireGame;
    }

    private void OnDisable()
    {
        inputToResetGame.action.performed -= OnInputToResetEntireGame;
        inputToResetGame.action.Disable();
    }

    private void OnInputToResetEntireGame(InputAction.CallbackContext context)
    {
        ResetAllPuzzlesSolved.ResetPuzzlesSolved();
        wordsCollected.Clear();
        wordsCollected.TrimExcess();

        SceneManager.LoadSceneAsync(titleScreenSceneIndex);
    }


    public static IReadOnlyCollection<WordData> GetAllWordsPlayerHasCollected()
    {
        return instance.wordsCollected;
    }

    public static IReadOnlyCollection<NounData> GetAllNounsPlayerHasCollected()
    {
        List<NounData> playerNouns = new();
        instance.wordsCollected.ForEach(x =>
        {
            if (x is NounData y) playerNouns.Add(y);
        });
        return playerNouns;
    }

    public static bool TryAddWordToWordsCollected(WordData localWord)
    {
        if (localWord == null) return false;
        if (instance.wordsCollected.Contains(localWord)) return false;

        instance.wordsCollected.Add(localWord);
        return true;
    }

    public static void PlayOneShotOnEnterCraftingMenu()
    {
        instance.soundEnterCraftingMenu.Play();
    }

    public static void PlayOneShotOnClickOnWord()
    {
        instance.soundClickOnWord.Play();
    }

    public static void PlayOneShotOnClickOnCancelButton()
    {
        instance.soundClickOnCancelButton.Play();
    }

    public static void PlaySittingOnSittableAnimation(ISittable sittable)
    {
        instance.actorIsSittingOn = sittable;
        instance.actorIsSittingOn.OnSitStart(instance.actorPlayerTransform);
    }

    public static void StandUpFromSittable()
    {
        instance.actorIsSittingOn.OnSitEnd(instance.actorPlayerTransform);
        instance.actorIsSittingOn = null;
    }

    public static void OnEnterMoveCameraToCaptureActorWithConversable(IConversable conversable)
    {
        /* TODO:
         * - Call "Camera Targets This" GameObject and move it between player and currentConversable
         * - Call the same object /method in that object to then set y value to a specific height up
         * - This would give space for the Dialog Box.
         * - Get WorldToScreenPosition from currentConversable to then add the "Triangle" Dialog thingy.
         */

        if (conversable is NpcBehaviour npcBehaviour)
        {
            Vector3 positionNpc = npcBehaviour.transform.position;
            Vector3 positionActor = instance.actorPlayerTransform.transform.position;

            Vector2 positionDifference = new(positionNpc.x - positionActor.x, positionNpc.z - positionActor.z);
            float magnitude = positionDifference.magnitude * 0.5f;

            positionDifference = positionDifference.normalized * magnitude;

            instance.cameraFollowTarget.SetTargetTransformPosition(positionDifference.x, Vector3.up.y,
                positionDifference.y);
        }
    }

    public static void OnExitMoveCameraToCaptureActorWithConversable(IConversable conversable)
    {
        ResetCameraToOriginalPosition();
    }

    public static void MoveCameraToAFocus(Transform target)
    {
        Vector3 distance = target.position - instance.actorPlayerTransform.position;
        instance.cameraFollowTarget.SetTargetTransformPosition(distance);
    }

    public static void ResetCameraToOriginalPosition()
    {
        instance.cameraFollowTarget.SetTargetTransformPosition(Vector3.zero);
    }

    public static Vector3 GetActorPosition()
    {
        return instance.actorPlayerTransform.transform.position;
    }

    public static Vector3 GetCameraTargetPosition()
    {
        return instance.cameraFollowTarget.GetTargetTransformPosition();
    }

    public static Material GetOutlineMaterialWhenInspecting()
    {
        return instance.inspectHighlightMat;
    }
}
