using System.Collections.Generic;

public interface IConversable
{
    // A Conversable Object has the ability to do some ChitChat and be able to give Talking/Prompts to where the player can give a response
    //void LoadChitChat();
    bool StartChitChat();
    bool NextChitChat();

    bool StartTalkPrompt();
    bool TryResponse(List<WordData> sentence);
    void StartSolutionChitChat();
    void StartWrongResponseChitChat();
    SentenceData GetSolutionSentence();
}
