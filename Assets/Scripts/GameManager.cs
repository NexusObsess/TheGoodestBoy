using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentDay = 0;
    public float TownMorale = 100;
    public float TownMoraleDailyDecrease;

    [Header ("Manager Scripts")]
    QuestManager questManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // opening sequence
        // get any necessary manager scripts to call functions
        questManager = FindFirstObjectByType<QuestManager>();
    }

    public void NewDay()
    {
        questManager.GiveQuestLines();

        currentDay ++; // keep at bottom
    }

    public void EndDay()
    {
        TownMorale -= TownMoraleDailyDecrease;
        questManager.EndQuestCheck();

        if (TownMorale >= 90)
        {
            questManager.QuestResultText.text += "\n\nThe people of the town are in high spirits! The streets are full of friendly smiling faces who give you many pets and treats.";
        }
        else if (TownMorale >= 60)
        {
            questManager.QuestResultText.text += "\n\nThe people of the town are slightly nervous. The streets seem less lively and when people approach you with pets and treats, they also ask after the knight's health.";
        }
        else if (TownMorale >= 30)
        {
            questManager.QuestResultText.text += "\n\nThe people of the town are very tense. Very few sorry looking townspeople are still out by the time you get back and they all walk in groups. The baker asks if the town should prepare for the worst.";
        }
        else
        {
            questManager.QuestResultText.text += "\n\nYou don't see anyone on the way into town, aside from the woodworker fortifying the end of the town leading into the forest who doesn't look at you. The buildings are boarded up and you get no treats.";
        }
    }
}