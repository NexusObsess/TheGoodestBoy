using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public int questdifficulty;

    public bool questCompleted = false;

    public enum questType { Fetch, Hunt, Talk, Location, Puzzle, Challenge }
    public questType QuestType;

    public enum questLine { None, Main, Baker }
    public questLine QuestLine;

    // see if can hide when not relevant based on questtype
    [Header ("For Talk")]
    public string questID;

    [Header ("For Hunt")]
    public int requiredAmount;
    public int currentAmount;

    [Header ("For QuestLines")]
    public int QuestOrder;
    public int CurrentQuestFromLine; // move to quest manager;
}