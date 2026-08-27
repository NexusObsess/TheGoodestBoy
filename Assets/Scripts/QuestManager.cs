using UnityEngine;
using System.Collections.Generic; 
using System.Linq;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    //List<Quest> toRemoveQuests = new List<Quest>();
    [SerializeField] List<Quest> possibleQuests = new List<Quest>();

    List<QuestLine> questLines = new List<QuestLine>();

    GameManager gameManager;

    public TextMeshProUGUI QuestResultText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Quest.questLine line in System.Enum.GetValues(typeof(Quest.questLine))) // find questline enums from quest script
        {
            if (line == Quest.questLine.None) continue; // to the none questline
            questLines.Add(new QuestLine(line)); // adds each questline to an entry of QuestLine
        }

        gameManager = FindFirstObjectByType<GameManager>();

        SortQuestLines();
    }

    void SortQuestLines()
    {
        foreach (Quest quest in possibleQuests) // repeats every possible quest
        {
            if (quest.QuestLine == Quest.questLine.None) continue; // doesn't apply when the quest is not part of a questline

            QuestLine questLine = questLines.Find(ql => ql.lineType == quest.QuestLine); // finds the questline the quest should go in
            questLine.quests.Add(quest); // adds the quest
        }

        foreach (QuestLine questLine in questLines) // Sort every questline by questOrder
        {
            questLine.quests = questLine.quests
                .OrderBy(q => q.QuestOrder) // reorganizes list by QuestOrder int
                .ToList(); // sets new list order
        }
    }

    public Quest GetNextQuest(Quest.questLine line)
    {
        QuestLine questLine = questLines.Find(ql => ql.lineType == line); // find the questline that matches what was sent in brackets i.e., main

        if (questLine.quests == null) return null; // for debug

        // Debug.Log("Get past return");

        foreach (Quest quest in questLine.quests) // repeat for each quest in questLine list
        {
            if (!quest.questCompleted) // is the quest complete?
            {
                return quest; // send the first quest that is not complete
            }
        }
        // All quests in this questline are completed
        return null;
    }

    public Quest LookForTrackingQuestLines(Quest.questLine line)
    {
        QuestLine questLine = questLines.Find(ql => ql.lineType == line); // find the questline that matches what was sent in brackets i.e., main

        if (questLine.quests == null) return null;

        if (questLine.quests[0].questCompleted == true) // check if player has started questline 
        {
            Quest nextSideQuest = LookForTrackingQuestLines(Quest.questLine.Baker);
        }

        return null;
    }

    public void GiveQuestLines()
    {
        Quest nextMainQuest = GetNextQuest(Quest.questLine.Main);
        if (nextMainQuest != null)
        {
            activeQuests.Add(nextMainQuest);
            possibleQuests.Remove(nextMainQuest);
        }

        // look for tracking quest lines
        foreach (Quest quest in possibleQuests)
        {
            if (quest.QuestLine == Quest.questLine.Main || quest.QuestLine == Quest.questLine.None) continue;

            Quest nextSideQuest = LookForTrackingQuestLines(quest.QuestLine);

            if (nextSideQuest != null && activeQuests.Count < 3)
            {
                activeQuests.Add(nextSideQuest);
                possibleQuests.Remove(nextSideQuest);
            }
        }

        SetSideQuests();
    }

    void SetSideQuests()
    {
        //Debug.Log("Set Side Quests");

        if (activeQuests.Count < 3)
        {
            //Debug.Log("After Quest Count");
            int randomIndex = Random.Range(0, possibleQuests.Count); // maxExclusive
            // Debug.Log(randomIndex);
            Quest randomSideQuest = possibleQuests[randomIndex];

            if (randomSideQuest.questDifficulty <= gameManager.currentDay && randomSideQuest.QuestLine != Quest.questLine.Main & randomSideQuest.QuestOrder != 0) // check difficulty and questline
            {
                activeQuests.Add(randomSideQuest);
                possibleQuests.Remove(randomSideQuest);
            }

            SetSideQuests();
        }
        else
        {
            // progress
        }
    }

    public void EndQuestCheck()
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.questCompleted == true)
            {
                Debug.Log(quest.questName + " complete!");
                // add quest win string to Quest Result text
                QuestResultText.text += "\n" + quest.questWon;
                gameManager.TownMorale += quest.townMoraleIncrease;
            }
            else
            {
                Debug.Log(quest.questName + " incomplete :(");
                // add quest lose string to Quest Result text
                QuestResultText.text += "\n" + quest.questFailed;
            }

            // randomSideQuest.QuestLine != Quest.questLine.Main
            if (quest.QuestLine != Quest.questLine.None)
            {
                CompleteQuestLines(quest.QuestLine, quest);
            }
        }

        activeQuests = null;
    }

    public void CompleteQuestLines(Quest.questLine line, Quest checkQuest)
    {
        QuestLine questLine = questLines.Find(ql => ql.lineType == line); // find the questline that matches what was sent in brackets i.e., main

        foreach (Quest quest in questLine.quests) // repeat for each quest in questLine list
        {
            if (checkQuest != quest) return;
            Debug.Log("Found match");

            if (checkQuest.questCompleted == true)
            {
                quest.questCompleted = true;
            }
            else
            {
                if (checkQuest.QuestLine == Quest.questLine.Main) return;
                RemoveQuestLine(questLine);
            }
        }
    }

    void RemoveQuestLine(QuestLine questLine)
    {
        questLine.quests = null;
    }
}

[System.Serializable]
public class QuestLine
{
    public Quest.questLine lineType; // which quest line does each entry represent i.e., main, baker etc
    public List<Quest> quests = new List<Quest>(); // quests from the quest line

    public QuestLine(Quest.questLine lineType) // sets the QuestLine based on the finding in start
    {
        this.lineType = lineType;
    }
}