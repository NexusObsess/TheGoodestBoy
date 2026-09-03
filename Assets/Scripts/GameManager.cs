using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int currentDay = 0;
    public float TownMorale = 100;
    public float TownMoraleDailyDecrease;

    [SerializeField] TextMeshProUGUI StatText;

    [Header ("Manager Scripts")]
    QuestManager questManager;
    [SerializeField] TextTreeChooser EndTownMorale;

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
        UpdatePauseStats();
    }

    public void EndDay()
    {
        TownMorale -= TownMoraleDailyDecrease;
        questManager.EndQuestCheck();
        UpdatePauseStats();

        EndTownMorale.SelectCorrectTextTree();
    }

    void UpdatePauseStats()
    {
        StatText.text = "Town Morale: " + TownMorale + "\nCurrent day: " + currentDay;
    }
}