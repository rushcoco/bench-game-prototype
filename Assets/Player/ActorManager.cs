using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private static ActorManager instance;
    [SerializeField] private List<WordData> wordsCollected;
    [SerializeField] private VerbData toHave;
    [SerializeField] private VerbData toBe;
    [SerializeField] private VerbData will;
    [SerializeField] private AudioSource soundClickOnWord;
    [SerializeField] private AudioSource soundClickOnCancelButton;
    [SerializeField] private AudioSource soundEnterCraftingMenu;
    [SerializeField] private AudioSource soundClickOnCraftButton;
    [SerializeField] private AudioSource soundOnInspect;
    [SerializeField] private GameObject actorGameObject;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (wordsCollected is null)
            wordsCollected = new List<WordData>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public static ActorManager Instance()
    {
        return instance;
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
        instance.actorIsSittingOn.OnSitStart(instance.actorGameObject.transform);
    }

    public static void StandUpFromSittable()
    {
        instance.actorIsSittingOn.OnSitEnd(instance.actorGameObject.transform);
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
            Vector3 positionActor = instance.actorGameObject.transform.position;

            Vector2 positionDifference = new(positionNpc.x - positionActor.x, positionNpc.z - positionActor.z);
            float magnitude = positionDifference.magnitude * 0.5f;

            positionDifference = positionDifference.normalized * magnitude;

            instance.cameraFollowTarget.SetTargetTransformPosition(positionDifference.x, Vector3.up.y,
                positionDifference.y);
        }
    }

    public static void OnExitMoveCameraToCaptureActorWithConversable(IConversable conversable)
    {
        instance.cameraFollowTarget.SetTargetTransformPosition(Vector3.zero);
    }

    public static Vector3 GetActorPosition()
    {
        return instance.actorGameObject.transform.position;
    }
}
