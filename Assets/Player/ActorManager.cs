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

    private void Awake()
    {
        if (!instance.IsUnityNull() && instance != this)
            Destroy(this);
        else
            instance = this;

        if (wordsCollected == null)
            wordsCollected = new List<WordData>();

        VerbConjugator.SetAuxiliaryVerbs(toHave, toBe, will);
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
}
