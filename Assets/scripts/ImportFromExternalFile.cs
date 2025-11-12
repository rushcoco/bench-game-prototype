using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ImportFromExternalFile
{
    private const string ROOT_PATH = "Assets/Resources/";
    private static List<WordData> listWordData;
    private static List<SentenceData> listSentenceData;
    private static List<PuzzleData> listPuzzleData;
    private static Dictionary<int, List<string>> chitChats;
    private static Dictionary<int, List<string>> solvedPuzzleDialogues;

#if UNITY_EDITOR
    [MenuItem("Tools/Import Everything")]
    public static void ImportEverythingAtOnce()
    {
        ImportChitChat();
        ImportSolvedDialogue();
        ImportWords();
        ImportSentences();
        ImportPuzzle();
    }

    [MenuItem("Tools/Import Sentences")]
    public static void ImportSentences()
    {
        // What do i need?
        // - path to csv file
        // - read content of the file
        // - make sure the file is built correctly
        // - create puzzle scriptableobject assets based on the values
        // - set these assets to npc
        // - puzzle needs correct sentence
        // - correct sentence need word assets and references to those words
        // - do that with id?
        // ---------------------------------------------------------

        string pathFile = EditorUtility.OpenFilePanel("Select SentenceData CSV", "", "csv");
        if (string.IsNullOrEmpty(pathFile)) return;

        string pathFolder = ROOT_PATH + "sentence-assets";

        string[] lines = File.ReadAllLines(pathFile);

        listSentenceData = new List<SentenceData>();

        if (!Directory.Exists(pathFolder))
            Directory.CreateDirectory(pathFolder);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // first three are set and then afterwords are unspecified amount of numbers (linked to wordsId)
            SentenceData sentenceData = ScriptableObject.CreateInstance<SentenceData>();
            sentenceData.id = int.Parse(values[0]);
            if (Enum.TryParse(values[1], out Tense tense))
                sentenceData.tense = tense;
            if (Enum.TryParse(values[2], out Pronoun pronoun))
                sentenceData.pronoun = pronoun;

            sentenceData.words = new List<WordData>();

            for (int j = 3; j < values.Length; j++)
                if (int.TryParse(values[j], out int result))
                    sentenceData.words.Add(listWordData.Find(x => x.id == result));

            sentenceData.SetSentence();
            string assetPath = $"{pathFolder}/{sentenceData.id}.asset";
            AssetDatabase.CreateAsset(sentenceData, assetPath);
            listSentenceData.Add(sentenceData);
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Import completed");
    }

    [MenuItem("Tools/Import Words From CSV")]
    public static void ImportWords()
    {
        string pathFile = EditorUtility.OpenFilePanel("Select Word CSV", "", "csv");
        if (string.IsNullOrEmpty(pathFile)) return;

        const string pathFolder = ROOT_PATH + "word-assets/";
        string pathFolderVerb = pathFolder + "verb";
        string pathFolderNoun = pathFolder + "noun";
        string pathFolderAdj = pathFolder + "adjective";
        string pathFolderPro = pathFolder + "pronoun";
        string pathFolderOth = pathFolder + "other";

        listWordData = new List<WordData>();

        string[] lines = File.ReadAllLines(pathFile);

        if (!Directory.Exists(pathFolderVerb))
            Directory.CreateDirectory(pathFolderVerb);
        if (!Directory.Exists(pathFolderNoun))
            Directory.CreateDirectory(pathFolderNoun);
        if (!Directory.Exists(pathFolderAdj))
            Directory.CreateDirectory(pathFolderAdj);
        if (!Directory.Exists(pathFolderPro))
            Directory.CreateDirectory(pathFolderPro);
        if (!Directory.Exists(pathFolderOth))
            Directory.CreateDirectory(pathFolderOth);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            switch (int.Parse(values[2]))
            {
                case 0:
                {
                    if (AssetDatabase.AssetPathExists($"{pathFolderVerb}/{values[1]}.asset")) continue;
                    ImportVerb(values, pathFolderVerb);
                    break;
                }
                case 1:
                {
                    if (AssetDatabase.AssetPathExists($"{pathFolderNoun}/{values[1]}.asset")) continue;
                    ImportNoun(values, pathFolderNoun);
                    break;
                }
                case 2:
                {
                    if (AssetDatabase.AssetPathExists($"{pathFolderAdj}/{values[1]}.asset")) continue;
                    ImportAdjective(values, pathFolderAdj);
                    break;
                }
                case 3:
                {
                    if (AssetDatabase.AssetPathExists($"{pathFolderPro}/{values[1]}.asset")) continue;
                    ImportPronoun(values, pathFolderPro);
                    break;
                }
                case 4:
                {
                    if (AssetDatabase.AssetPathExists($"{pathFolderOth}/{values[1]}.asset")) continue;
                    ImportOtherWord(values, pathFolderOth);
                    break;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Import completed");
    }

    private static void ImportNoun(string[] values, string pathFolder)
    {
        NounData wordData = ScriptableObject.CreateInstance<NounData>();
        wordData.id = int.Parse(values[0]);
        wordData.presentedWord = values[1];

        string assetPath = $"{pathFolder}/{wordData.presentedWord}.asset";
        AssetDatabase.CreateAsset(wordData, assetPath);
        listWordData.Add(wordData);
    }

    private static void ImportVerb(string[] values, string pathFolder)
    {
        VerbData wordData = ScriptableObject.CreateInstance<VerbData>();
        wordData.id = int.Parse(values[0]);
        wordData.presentedWord = values[1];

        wordData.infinitive = values[3];
        wordData.imperative = values[4];
        wordData.present[0] = values[5].Trim('"');
        wordData.present[1] = values[6];
        wordData.present[2] = values[7];
        wordData.present[3] = values[8];
        wordData.present[4] = values[9];
        wordData.present[5] = values[10].Trim('"');
        wordData.preterite[0] = values[11].Trim('"');
        wordData.preterite[1] = values[12];
        wordData.preterite[2] = values[13];
        wordData.preterite[3] = values[14];
        wordData.preterite[4] = values[15];
        wordData.preterite[5] = values[16].Trim('"');

        wordData.presentParticiple = values[17];
        wordData.pastParticiple = values[18];

        string assetPath = $"{pathFolder}/{wordData.presentedWord}.asset";
        AssetDatabase.CreateAsset(wordData, assetPath);
        listWordData.Add(wordData);
    }

    private static void ImportAdjective(string[] values, string pathFolder)
    {
        AdjectiveData wordData = ScriptableObject.CreateInstance<AdjectiveData>();
        wordData.id = int.Parse(values[0]);
        wordData.presentedWord = values[1];

        wordData.formBase = values[9];
        wordData.formMore = values[10];
        wordData.formMost = values[11];
        wordData.hasOnlyBaseForm = bool.Parse(values[12]);

        string assetPath = $"{pathFolder}/{wordData.presentedWord}.asset";
        AssetDatabase.CreateAsset(wordData, assetPath);
        listWordData.Add(wordData);
    }

    private static void ImportPronoun(string[] values, string pathFolder)
    {
        PronounData wordData = ScriptableObject.CreateInstance<PronounData>();
        wordData.id = int.Parse(values[0]);
        wordData.presentedWord = values[1];

        wordData.pronoun = Enum.Parse<Pronoun>(values[13]);

        string assetPath = $"{pathFolder}/{wordData.presentedWord}.asset";
        AssetDatabase.CreateAsset(wordData, assetPath);
        listWordData.Add(wordData);
    }

    private static void ImportOtherWord(string[] values, string pathFolder)
    {
        OtherData wordData = ScriptableObject.CreateInstance<OtherData>();
        wordData.id = int.Parse(values[0]);
        wordData.presentedWord = values[1];

        string assetPath = $"{pathFolder}/{wordData.presentedWord}.asset";
        AssetDatabase.CreateAsset(wordData, assetPath);
        listWordData.Add(wordData);
    }

    [MenuItem("Tools/Import Puzzles")]
    public static void ImportPuzzle()
    {
        listPuzzleData = new List<PuzzleData>();

        string pathFile = EditorUtility.OpenFilePanel("Select PuzzleData CSV", "", "csv");
        if (string.IsNullOrEmpty(pathFile)) return;

        string pathFolder = ROOT_PATH + "puzzle-assets";

        string[] lines = File.ReadAllLines(pathFile);

        if (!Directory.Exists(pathFolder))
            Directory.CreateDirectory(pathFolder);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            PuzzleData puzzleData = ScriptableObject.CreateInstance<PuzzleData>();
            puzzleData.id = int.Parse(values[0]);
            if (int.TryParse(values[1], out int sentenceKey))
                puzzleData.correctSentenceData = listSentenceData.Find(x => x.id == sentenceKey);

            if (int.TryParse(values[2], out int chitChatKey))
                puzzleData.dialogChitChat = chitChats[chitChatKey];
            if (int.TryParse(values[3], out int solvedDialoguesKey))
                puzzleData.dialogPuzzleSolved = solvedPuzzleDialogues[solvedDialoguesKey];

            puzzleData.dialogResponseFalse = values[4];
            puzzleData.dialogTimeRunOut = values[5];
            puzzleData.dialogPuzzlePrompt = values[6];

            if (int.TryParse(values[7], out int result))
                puzzleData.timeLimitInSeconds = result;


            string assetPath = $"{pathFolder}/{puzzleData.id}.asset";
            AssetDatabase.CreateAsset(puzzleData, assetPath);
            listPuzzleData.Add(puzzleData);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Import Worked");
    }

    [MenuItem("Tools/Import ChitChat")]
    public static void ImportChitChat()
    {
        chitChats = new Dictionary<int, List<string>>();
        ImportStandardFileToDictionary("Select Chit Chat File", "", "csv", chitChats);
        Debug.Log("Imported ChitChat");
    }

    [MenuItem("Tools/Import PuzzleData Solved Dialogue")]
    public static void ImportSolvedDialogue()
    {
        solvedPuzzleDialogues = new Dictionary<int, List<string>>();
        ImportStandardFileToDictionary("Select Puzzle Solved Dialogue", "", "csv", solvedPuzzleDialogues);
        Debug.Log("Imported Solved Dialogue Chats");
    }

    private static void ImportStandardFileToDictionary(string title, string directory, string extension,
        Dictionary<int, List<string>> dictionary)
    {
        string pathFile = EditorUtility.OpenFilePanel(title, directory, extension);
        if (string.IsNullOrEmpty(pathFile)) return;

        string[] lines = File.ReadAllLines(pathFile);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            dictionary.Add(int.Parse(values[0]), new List<string>());

            for (int j = 1; j < values.Length; j++)
            {
                if (string.IsNullOrEmpty(values[j])) break;

                if (values[j].Contains('"'))
                    values[j] = values[j].Trim('"');

                dictionary[int.Parse(values[0])].Add(values[j]);
            }
        }
    }

    private static void ClearFolder(string path, Type type)
    {
    }
#endif
}
