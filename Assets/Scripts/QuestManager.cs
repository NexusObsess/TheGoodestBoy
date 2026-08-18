using UnityEngine;
using System.Collections.Generic; 

public class QuestManager : MonoBehaviour
{
    //public static QuestManager instance;

    public List<Quest> activeQuests = new List<Quest>();
    //public List<Quest> completedQuests = new List<Quest>();
    [SerializeField] List<Quest> possibleQuests = new List<Quest>();

    public List<Quest> questLineMain = new List<Quest>();
    List<List<Quest>> QuestLines;

    int currentDay = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // add all questline lists to QuestLines list
        //QuestLines.Add(questLineMain);
        SortQuestLinesSender();
    }

    void SortQuestLinesSender()
    {
        foreach (Quest q in possibleQuests)
        {
            for (int i = 0; i < possibleQuests.Count; i++)
            {
                if (possibleQuests[i].QuestLine != Quest.questLine.None)
                {
                    //questLineMain.Add(possibleQuests[i].QuestOrder);
                    questLineMain[possibleQuests[i].QuestOrder] = possibleQuests[i];
                }
            }
        }
    }

    void NewDay()
    {
        currentDay ++;
        // add next main quest first
        SetSideQuests();
    }

    void SetSideQuests()
    {
        //Debug.Log("Set Side Quests");
        // look for tracking quest lines
        if (activeQuests.Count < 3)
        {
            //Debug.Log("After Quest Count");
            int randomIndex = Random.Range(0, possibleQuests.Count); // maxExclusive
            Quest randomSideQuest = possibleQuests[randomIndex];

            if (randomSideQuest.questDifficulty == currentDay) //&& randomSideQuest.QuestOrder == randomSideQuest.CurrentQuestFromLine) // check difficulty and questline
            {
                activeQuests.Add(randomSideQuest);
                possibleQuests.Remove(randomSideQuest);
            }

            SetSideQuests();
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
