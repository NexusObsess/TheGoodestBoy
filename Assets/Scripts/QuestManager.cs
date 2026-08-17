using UnityEngine;
using System.Collections.Generic; 

public class QuestManager : MonoBehaviour
{
    //public static QuestManager instance;

    public List<Quest> activeQuests = new List<Quest>();
    //public List<Quest> completedQuests = new List<Quest>();
    [SerializeField] List<Quest> possibleQuests = new List<Quest>();

    List<Quest> QuestLineMain = new List<Quest>();
    //List<List<Quest>> QuestLines;

    int currentDay = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // add all questline lists to QuestLines list

        //QuestLines.Add(QuestLineMain);
        //SortQuestLinesSender();

        SortQuestLine();

        SetSideQuests();
    }

    void SortQuestLine()
    {
        // check int of each quest in questline
        // place them in order by this int
    }

    // void SortQuestLinesSender()
    // {
    //     SortQuestLinesReciever(QuestLineMain);
    // }

    // void SortQuestLinesReciever(List<Quest> QuestLineMain)
    // {
        
    // }

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

            if (randomSideQuest.questdifficulty == currentDay && randomSideQuest.QuestOrder == randomSideQuest.CurrentQuestFromLine) // check difficulty and questline
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
