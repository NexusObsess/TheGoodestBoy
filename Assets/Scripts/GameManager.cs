using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int currentDay = 0; // what ingame day is it - used for determining the difficulty of the quests the player can get and for ui
    public float TownMorale = 100; // determines game ending and some flavour text
    public float TownMoraleDailyDecrease; // how much the morale of the town decreases every ingame day
    public float KnightHealth = 6;
    public float KnightHealthDailyDecrease;

    [SerializeField] TextMeshProUGUI StatText; // text on pause menu displaying the current day, town morale etc

    [Header ("Manager Scripts")]
    QuestManager questManager; // manages quest system

    [Header ("Text trees")]
    public GameObject textboxobject;
    TextBoxSender textboxsender;
    [SerializeField] TextTreeChooser EndOfDayVariations;
    // [SerializeField] TextTreeChooserMorale EndTownMorale; // picks which text tree checking in on the town based on the town morale int at the end of every ingame day
    // [SerializeField] TextTreeChooserKnightHealth EndKnightHealth; // picks which text tree checking in on the town based on the town morale int at the end of every ingame day
    public bool TextActive = false;
    public bool GameOverTriggered = false;

    void Start()
    {
        // get any necessary manager scripts to call functions
        questManager = FindFirstObjectByType<QuestManager>();
        textboxsender = FindFirstObjectByType<TextBoxSender>(); // again, just in case

        // opening sequence
        // call newday()
    }

    public void NewDay() // called every ingame day after the end of day sequence finishes and the opening sequence
    {
        // spawns player in starting location healed(?)
        // loads scenes if needed

        // create new version of 'dungeon' ?

        questManager.GiveQuestLines(); // starts the process of giving the player the daily quests

        currentDay ++; // keep at bottom, for quest system
        UpdatePauseStats(); // call everytime one of the stats changes
    }

    public void EndDay() // called at the end of every ingame day or if player leaves dunegon
    {
        // stop 'dungeon' stuff
        TownMorale -= TownMoraleDailyDecrease; // daily decrease of town morale
        KnightHealth -= KnightHealthDailyDecrease * currentDay;

        questManager.EndQuestCheck(); // checks if each quest was completed and then sends the corrosponding fail or win text into the end of day sequence text
        UpdatePauseStats();

        //EndTownMorale.SelectCorrectTextTree(); // checks which text tree to send based on town morale at the end of every ingame day
        EndOfDayVariations.MoraleSelectCorrectTextTree();
        EndOfDayVariations.KnightHealthSelectCorrectTextTree();
        // visuals

        textboxsender.DialogueSequenceStarts(); // may need to move?
        StartCoroutine(TextBoxCheck());
        // add a corountine that waits until the textbox is inactive again before starting a new day with yield return new WaitUntil(() => bool true); but idk
    }

    IEnumerator TextBoxCheck()
    {
        yield return new WaitUntil(() => !TextActive);
        Debug.Log("TEXTBOX IS OVER PARTY");
        textboxobject.SetActive(false);
        
        if (GameOverTriggered == true)
        {
            // trigger game over screen
            Debug.Log("Game over screen load");
        }
    }

    void UpdatePauseStats()
    {
        StatText.text = "Town Morale: " + TownMorale + "\nCurrent day: " + currentDay; // \n skips a line
    }
}