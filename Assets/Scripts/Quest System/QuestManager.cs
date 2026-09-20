using UnityEngine;
using System.Collections.Generic; 
using System.Linq;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>(); // quests the player gets that ingame day
    [SerializeField] List<Quest> possibleQuests = new List<Quest>(); // pool of possible quests the player can get, unsorted into questlines
    List<QuestLine> questLines = new List<QuestLine>(); // list of the system.serialized at bottom of script

    GameManager gameManager;

    MailBox mailbox;

    [SerializeField] TextBoxSender QuestResults; // sends the quest win or lose text to textbox

    // ui text in pause menu where the player can read about their current quests during game play
    [SerializeField] List<TextMeshProUGUI> QuestName = new List<TextMeshProUGUI>(); 
    [SerializeField] List<TextMeshProUGUI> QuestDescription = new List<TextMeshProUGUI>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Quest.questLine line in System.Enum.GetValues(typeof(Quest.questLine))) // find questline enums from quest script
        {
            if (line == Quest.questLine.None) continue; // to the none questline
            questLines.Add(new QuestLine(line)); // adds each questline to an entry of QuestLine
        }

        gameManager = FindFirstObjectByType<GameManager>();
        mailbox = FindFirstObjectByType<MailBox>();

        SortQuestLines();
    }

    void SortQuestLines()
    {
        foreach (Quest quest in possibleQuests) // repeats every possible quest
        {
            if (quest.QuestLine == Quest.questLine.None) continue; // doesn't apply when the quest is not part of a questline

            QuestLine questLine = questLines.Find(ql => ql.lineType == quest.QuestLine); // finds the questline the quest should go in the system.serialized list
            questLine.quests.Add(quest); // adds the quest
        }

        foreach (QuestLine questLine in questLines) // Sort every questline by questOrder using System.Linq
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

        if (questLine.quests == null) return null; // if questline has not been failed or is empty, mostly for debug

        if (questLine.quests[0].questCompleted == true) // check if player has started questline 
        {
            Quest nextSideQuest = LookForTrackingQuestLines(Quest.questLine.Baker); // if they have, get next quest
        }

        return null;
    }

    public void GiveQuestLines() // called from game manager
    {
        // if (Quest.questLine.Main == null) // debug
        // {
        //     Debug.LogError("Quest.questLine.Main is null");
        //     return;
        // }

        Quest nextMainQuest = GetNextQuest(Quest.questLine.Main);

        if (nextMainQuest != null) // if you actually get main quest, mostly for preventing errors during prototypimg
        {
            // Debug.Log(nextMainQuest.questName);
            // Debug.Log(activeQuests);
            activeQuests.Add(nextMainQuest); // add to active quest
            possibleQuests.Remove(nextMainQuest); // remove from the random quest pool
        }

        foreach (Quest quest in possibleQuests) // look for tracking quest lines
        {
            if (quest.QuestLine == Quest.questLine.Main || quest.QuestLine == Quest.questLine.None) continue; // if quest is not in main questline OR has no questline

            Quest nextSideQuest = LookForTrackingQuestLines(quest.QuestLine); // give next quest that is not complete in line

            if (nextSideQuest != null && activeQuests.Count < 3) // quest is not null and there are less than three active quests
            {
                activeQuests.Add(nextSideQuest);
                possibleQuests.Remove(nextSideQuest);
            }
        }

        SetSideQuests();
    }

    void SetSideQuests()
    {
        if (activeQuests.Count < 3) // repeats function until true
        {
            int randomIndex = Random.Range(0, possibleQuests.Count); // create random number between 1 and the total number of possible quests
            Quest randomSideQuest = possibleQuests[randomIndex]; // get possible quest

            // checks if difficulty and questline of quest match, if not skip to repeating function
            if (randomSideQuest.questDifficulty <= gameManager.currentDay && randomSideQuest.QuestLine != Quest.questLine.Main && randomSideQuest.QuestOrder == 0)
            {
                activeQuests.Add(randomSideQuest);
                possibleQuests.Remove(randomSideQuest);
            }
            else
            {
                Debug.Log(randomSideQuest.questDifficulty + randomSideQuest.QuestLine + randomSideQuest.QuestOrder);
            }
            SetSideQuests(); // starts function again
        }
        else // after getting three quests
        {
            for (int i = 0; i < activeQuests.Count; i++) // repeats for every active quests
            {
                //Debug.Log("if loop check");
                //mailbox.QuestLetterSprite.Add(activeQuests[i].questLetter);
                //questLetters[i].SetPopUp(activeQuests[i].questLetter); // assigns one of the quest letter game objects to the sprite attached to the active quest
                mailbox.QuestLetterSprite.Add(new QuestLetter(activeQuests[i].questLetter, false));

                // so the player can see quests on pause menu
                QuestName[i].text = activeQuests[i].questName;
                QuestDescription[i].text = activeQuests[i].questDescription;
            }
        }
    }

    public void HuntCheck (Enemy enemy)
    { 
       foreach (Quest q in activeQuests)
       {  
            if (q.QuestType == Quest.questType.Hunt)
            { 
                if (enemy == q.requiredEnemy)
                { 
                    q.currentEnemyAmount ++;

                    if (q.requiredEnemyAmount == q.currentEnemyAmount)
                    {
                        q.questCompleted = true;
                    }
                }
            } 
        }
    }

    public void FetchCheck (Item item)
    {
       foreach (Quest q in activeQuests)
       {  
            if (q.QuestType == Quest.questType.Fetch)
            { 
                if (item == q.requiredItem)
                { 
                    q.currentItemAmount ++;

                    if (q.requiredItemAmount == q.currentItemAmount)
                    {
                        q.questCompleted = true;
                    }
                }
            } 
        } 
    }

    public void TalkCheck (NPC npc)
    {
       foreach (Quest q in activeQuests)
       {  
            if (q.QuestType == Quest.questType.Talk)
            { 
                if (npc.Name == q.questNPCID)
                { 
                    q.questCompleted = true;
                }
            } 
        } 
    }

    // Location check

    public void EndQuestCheck() // called after every ingame day
    {
        foreach (Quest quest in activeQuests) // repeat for every active quest
        {
            if (quest.questCompleted == true) // quest complete check
            {
                //Debug.Log(quest.questName + " complete!");
                QuestResults.DialogueTree.Add(new TextLine(null, 1, quest.questWon)); // add quest win string to Quest Result text tree

                // give player any quest rewards
                gameManager.TownMorale += quest.townMoraleIncrease;
                gameManager.KnightHealth += quest.knightHealthIncrease;
            }
            else
            {
                //Debug.Log(quest.questName + " incomplete :(");
                QuestResults.DialogueTree.Add(new TextLine(null, 1, quest.questFailed)); // add quest lose string to Quest Result text tree
            }

            if (quest.QuestLine != Quest.questLine.None) // if quest is part of a questline
            {
                CompleteQuestLines(quest.QuestLine, quest); // sends the questline and quest itself
            }
        }

        for (int i = 0; i < QuestName.Count; i++) // clears the active quests tab on the pause menu
        {
            QuestName[i].text = "";
            QuestDescription[i].text = "";
        }

        activeQuests.Clear(); // removes all activeQuests from the list
    }

    public void CompleteQuestLines(Quest.questLine line, Quest checkQuest) // so at the start of the next ingame day, the questline lists are up to date with what quests in the line are complete
    {
        QuestLine questLine = questLines.Find(ql => ql.lineType == line); // find the questline that matches what was sent in brackets i.e., main

        foreach (Quest quest in questLine.quests) // repeat for each quest in questLine list
        {
            if (checkQuest != quest) return; // if quest in the questline is not the quest sent, try again
            // Debug.Log("Found match"); // found quest in the questline that matches the quest sent

            if (checkQuest.questCompleted == true) // if the quest sent was complete, mark the corrosponding quest in the questline as complete too for the check next ingame day
            {
                quest.questCompleted = true;
            }
            else // quest incomplete
            {
                if (checkQuest.QuestLine == Quest.questLine.Main) return; // player cannot fail the main quest line, they just get the same quest again next ingame day
                RemoveQuestLine(questLine); // player fails questline and will no longer get subsequent quests in the quest line
            }
        }
    }

    public void ToggleComplete1()
    {
        if (activeQuests.Count != 0)
        {
            activeQuests[0].questCompleted = !activeQuests[0].questCompleted;
        }
    }

    public void ToggleComplete2()
    {
        if (activeQuests.Count != 0)
        {
            activeQuests[1].questCompleted = !activeQuests[1].questCompleted;
        }
    }

    public void ToggleComplete3()
    {
        if (activeQuests.Count != 0)
        {
            activeQuests[2].questCompleted = !activeQuests[2].questCompleted;
        }
    }

    void RemoveQuestLine(QuestLine questLine) // seperate because cannot edit list you are iterating on 
    {
        questLine.quests = null; // quest line quests removed
    }
}

[System.Serializable]
public class QuestLine // for private list
{
    public Quest.questLine lineType; // which quest line does each entry represent i.e., main, baker etc
    public List<Quest> quests = new List<Quest>(); // quests from the quest line

    public QuestLine(Quest.questLine lineType) // sets the QuestLine based on the finding in start
    {
        this.lineType = lineType;
    }
}