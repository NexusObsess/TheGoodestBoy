using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Quest
{
    [Header("Quest Basics")]
    public string questName;
    public string questDescription;
    public int questDifficulty;

    [Header("Quest Outcomes")]
    public bool questCompleted = false;
    public string questFailed;
    public string questWon;

    [Header("Quest Reward")]
    public GameObject itemReward;
    public int townMoraleIncrease;
    // stat buff?
    // money?

    //[Header("Quest Specifications")]

    public enum questType { Fetch, Hunt, Talk, Location, Puzzle, Challenge }
    public questType QuestType;

    public enum questLine { None, Main, Baker }
    public questLine QuestLine;
    questLine Selected;

    // see if can hide all variables under headings when not relevant based on questtype, originally tried to hide in inspector

    // for fetch
    [QuestType(questType.Fetch)] public GameObject requiredItem;
    [QuestType(questType.Fetch)] public int requiredItemAmount = 1;

    // for hunt
    [QuestType(questType.Hunt)] public int requiredEnemyAmount;
    [QuestType(questType.Hunt)] public int currentEnemyAmount;

    // for talk
    [QuestType(questType.Talk)] public string questNPCID;
    // maybe multiple in a list?
    // reference in NPC script

    // for Location
    [QuestType(questType.Location)] public GameObject requiredLocationRoom;
    // must spawn in forest that day

    // Puzzle
    [QuestType(questType.Puzzle)] public GameObject requiredPuzzleRoom;
    // reference in code
    // must spawn in forest that day

    // Challenges
    [QuestType(questType.Challenge)] public GameObject requiredChallengeRoom;
    // challenge conditions to spawn i.e., time limit, enemies


    // for quests in questlines
    [QuestLineType(Quest.questLine.Main, Quest.questLine.Baker)] public int QuestOrder; // the order the quests in a quest line appear
}