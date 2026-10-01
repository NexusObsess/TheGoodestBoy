using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [Header ("UI")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject UnPauseButton;
    [SerializeField] TextMeshProUGUI StatText; // text on pause menu displaying the current day, town morale etc

    [Header ("Basics")]
    public int currentDay = 0; // what ingame day is it - used for determining the difficulty of the quests the player can get and for ui
    public float TownMorale = 100; // determines game ending and some flavour text
    public float TownMoraleDailyDecrease; // how much the morale of the town decreases every ingame day
    public float KnightHealth = 6;
    public float KnightHealthDailyDecrease;

    [Header ("Manager Scripts")]
    QuestManager questManager; // manages quest system
    public GameObject Player;
    Vector2 PlayerSpawn;
    PlayerStats playerstats;

    [Header ("Text trees")]
    public GameObject textboxobject;
    TextBox textbox;
    TextBoxSender textboxsender;
    [SerializeField] TextTreeChooser EndOfDayVariations;

    public bool TextActive = false;
    public bool GameOverTriggered = false;
    public bool TrueEndingTriggered = false;

    [Header ("NPCs")]
    public NPC Knight;
    public NPC Witch;
    public NPC WannabeHero;
    public NPC Baker;
    public NPC RivalDog;
    public NPC Apothecary;

    [SerializeField] string dungeonScene;

    void Start()
    {
        if(current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);


            // get any necessary manager scripts to call functions

            textboxsender = FindFirstObjectByType<TextBoxSender>(); // again, just in case
            textbox = FindFirstObjectByType<TextBox>(); // again, just in case
            questManager = FindFirstObjectByType<QuestManager>();
            playerstats = FindFirstObjectByType<PlayerStats>();
            PlayerSpawn = Player.transform.position;
            // opening sequence
            NewDay();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        UnPauseButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(UnPauseButton);
    }

    public void UnPause()
    {
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        UnPauseButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PauseButton);
    }

    public void NewDay() // called every ingame day after the end of day sequence finishes and the opening sequence
    {

        // spawns player in starting location healed(?)
        playerstats.currentHealth = playerstats.maxHealth;
        //playerstats.healthbar.SetHealth(playerstats.currentHealth);
        Player.transform.position = PlayerSpawn;

        // loads scenes if needed

        questManager.mailbox.ResetMailBox();
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject g in npcs)
        {
            Destroy(g);
        }

        questManager.GiveQuestLines(); // starts the process of giving the player the daily quests
        Debug.Log("NewDay");
        currentDay ++; // keep at bottom, for quest system
        SceneManager.LoadScene("Dungeon", LoadSceneMode.Additive); // create new version of 'dungeon' ?

        UpdatePauseStats(); // call everytime one of the stats changes
    }

    public void EndDay() // called at the end of every ingame day or if player leaves dunegon
    {
        SceneManager.UnloadSceneAsync(dungeonScene);// stop 'dungeon' stuff
        foreach (var enemy in SpawnerScript.enemies) //removes leftover enemies
        {
            TownMorale -= 1;
            Destroy(enemy);
        }
        foreach (var questStuffs in QuestSpawner.QuestStuffs) //removes leftover quest things
        {
            Destroy(questStuffs);
        }
        TownMorale -= TownMoraleDailyDecrease; // daily decrease of town morale
        KnightHealth -= KnightHealthDailyDecrease * currentDay;

        questManager.EndQuestCheck(); // checks if each quest was completed and then sends the corrosponding fail or win text into the end of day sequence text
        if (TownMorale < 0)
        {
            TownMorale = 0;
        }
        UpdatePauseStats();

        textbox.BackgroundOn();
        EndOfDayVariations.MoraleSelectCorrectTextTree();
        if (currentDay != 5 || questManager.questLines[0].quests[4].questCompleted == false)
        {
            EndOfDayVariations.KnightHealthSelectCorrectTextTree();

            textboxsender.DialogueSequenceStarts();
            StartCoroutine(TextBoxCheck()); // add a corountine that waits until the textbox is inactive again before starting a new day with yield return new WaitUntil(() => bool true); but idk
        }
        else
        {
            TrueEndingDialogueSequence();
        }
    }

    IEnumerator TextBoxCheck()
    {
        yield return new WaitUntil(() => !TextActive);
        Debug.Log("TEXTBOX IS OVER PARTY");
        textbox.BackgroundOff();
        textboxobject.SetActive(false);
        
        if (GameOverTriggered == true)
        {
            // trigger game over screen
            Debug.Log("Game over screen load");
            SceneManager.LoadScene("GameOverQuestNotComplete");
        }

        if (TrueEndingTriggered == true)
        {
            // trigger ending screen
            Debug.Log("End screen load");
        }
    }

    void UpdatePauseStats()
    {
        StatText.text = "Town Morale: " + TownMorale + "\nCurrent day: " + currentDay; // \n skips a line
    }

    void TrueEndingDialogueSequence()
    {
        TrueEndingTriggered = true;

        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The next day, the knight is finally able to get out of bed. You nearly knock the knight over in your excitement!"));
        textboxsender.DialogueTree.Add(new TextLine(Apothecary, 1, "My medicine already seems to have improved your condition."));
        textboxsender.DialogueTree.Add(new TextLine(Apothecary, 1, "Please still take it easy for the next couple of days. But if you pass a few more checkups, I will clear you to start adventuring again."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "For the first time in a week, you walk to the edge of the forest with the knight. You keep stopping to look at the knight like you think this is all too good to be true. Despite nearly tripping over you multiple times, the knight only laughs at your antics."));

        // Town Morale variations
        if (TownMorale >= 90)
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "It seems the apothecary told everyone in town the good news. The streets are full of people enjoying a spontaneous festival celebrating the knight's recovery."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "When you pass, people cheer and spoil you with treats. You and the knight mingle with the excited townspeople."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "Wow, everyone in town seems so happy! I'll have to step up my game or else I'll be out of a job! Good job, boy."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The knight scratches you behind the ears. You momentarily get so happy you simply must have zoomies around the courtyard! The townspeople laugh at your antics."));
        }
        else if (TownMorale >= 60)
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "At first, the town just operates as usual, if slightly gloomier. However, once they see the knight up again, everyone visibly relaxes. All the stores you pass offer you both gifts of appreciation. They wish you luck on your adventure that day."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "Good job holding down the fort, boy. I knew you could! I know it must have been difficult on your own."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The knight uses your side as a set of drums. Your tail nearly knocks her over in your excitement."));
        }
        else if (TownMorale >= 30)
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "Only a few townspeople are out in public this early, still grouped together like a monster will jump out at any moment. When you pass, they greet you both, but their smiles seem a little forced, and they hurry away."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "In one group, the body builder mutters under her breath something about unreliable people. The knight tells her to repeat herself louder. Wisely, the body builder just apologizes and rushes away. Your ears fold down onto your head."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "Ugh, leave it boy. They're just... stressed, it's been a hard week for everybody. What matters is that you did that best you could, and now I'm feeling better."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "To distract you, the knight throws your ball a couple of times. The exercise helps to take your mind off of the town for now. Now that the knight is better, she can cheer everybody up anyway!"));
        }
        else
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The town is almost completely lifeless. The knight glances around all the boarded-up windows and empty streets, clearly concerned. Your pace slows as you tuck your tail."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1.5f, "Ah, don't worry too much about it! Taking care of a whole town by yourself is very hard, I'm sure you did the best you could! We'll make everyone feel safe again together; this is just a rough patch..."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "You hope the knight is right..."));
        }

        if (questManager.questLines[3].quests[1].questCompleted) // if all wannabe questline is complete
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "On the way to the forest, you run into the wannabe hero and his mother. She thanks you again for all of your help while he hides behind her long skirt. Then she nudges her son forward with a patient smile."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The wannabe hero reluctantly steps forward with something hidden behind his back. The knight takes a knee to meet him at eye level. You lay down as well."));
            textboxsender.DialogueTree.Add(new TextLine(WannabeHero, 0.5f, "Here! I know it's ugly, but I drew you two beating up all the bad guys. Sorry again for causing wrouble..."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The knight stifles a laugh and takes the drawing, tucking it into her breast plate. You imagine that she will pin the drawing to the fridge when you get home."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "Tell you what kid, why don't you join us on an adventure sometime? That way we can show you the ropes while keeping you safe."));
            textboxsender.DialogueTree.Add(new TextLine(WannabeHero, 1.5f, "Wow! Really? T-thank you, Knight!"));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "Of course! As long as you wait a couple years for your poor mother's sake."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The wannabe hero nods so eagerly that his little papier mache helmet nearly falls off. You part ways and continue to the forest."));
        }

        textboxsender.DialogueTree.Add(new TextLine(null, 1, "You and the knight eventually arrive at the forest."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The witch is standing at the edge of the forest, as if he was expecting you both. The knight instinctively places a hand on the scabbard of her sword, ready for a fight."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "However, you step between them and lie down with your paws stretched in front of you and your head down. You make sure to sigh very loudly. They both look down at you and then more grudgingly back at each other. The witch sighs less obnoxiously."));
        textboxsender.DialogueTree.Add(new TextLine(Witch, 2, "Sorry for like... cursing you, tin can, or whatever >:(  I was just... jealous that you basically had our dog all to yourself."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The knight's armour clanks loudly, but you make the saddest face possible (you practiced in the reflection of your food bowl that morning). Standing down for now, the knight folds her arms."));
        textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "I wish you had told me you wanted more time with him. You caused a lot of trouble for not just me with your little stunt."));
        textboxsender.DialogueTree.Add(new TextLine(Knight, 2, "Though... I suppose I haven't been very approachable. I've been worried about you being a bad influence, but he is your dog too..."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "They both stand in awkward silence. You nudge the knight's boot with your nose."));
        textboxsender.DialogueTree.Add(new TextLine(Knight, 2, "Would you like to... talk more over brunch or something? We clearly need to negotiate again because it cannot escalate to this point again."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The witch, very reluctantly, agrees. The three of you walk back into town in silence."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "You sneak off to the town's counselor's office. The counselor... eventually realizes that your tugging him by the leg across the waiting room means you want him to follow you."));

        if (questManager.questLines[2].quests[2].questCompleted) // if all rival dog questline is complete
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "On your way out of the office, your self-proclaimed rival waddles out of one of the adjacent rooms. The rival dog jumps into the air when he sees you."));
            textboxsender.DialogueTree.Add(new TextLine(RivalDog, 2, "Woof, woof. (Uh, hey... I took your advice?)"));
            textboxsender.DialogueTree.Add(new TextLine(RivalDog, 2, "Boof... boof, boof (It's actually been... really helpful. You were right, I was so scared of being a bad boy compared to you that I couldn't be a good boy in my own way!)"));
            textboxsender.DialogueTree.Add(new TextLine(RivalDog, 1, "Hoooowwwlllll! (I'm sorry for being so mean to you this week...)"));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The rival dog's ears lay flat, and his tail tucks. You lick the rival dog's face, accepting the apology. You lower your front legs down while keeping your butt in the air. You invite the rival dog to play with you any time."));
            textboxsender.DialogueTree.Add(new TextLine(RivalDog, 1, "Woof... woof woof! (R-really? I mean, of course! Figures you'd need my help after all!)"));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The rival dog tries to act proud, but his tail is wagging even more than yours is. You are glad to finally have another dog friend in town, but you have to focus back on your human friends."));
        }

        textboxsender.DialogueTree.Add(new TextLine(null, 1, "When you get home, the knight and the witch are sitting at the dinner table, not looking at each other. For the first time in what feels like forever, the knight and the witch are in the same room without trying to kill each other."));

        if (questManager.questLines[1].quests[2].questCompleted) // if all baker questline is complete
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The smiling baker serves the knight, the counselor and the witch some cheesecake and you a doggy biscuit. The knight bows overly formally, and the baker giggles like it was the funniest thing she had ever seen. Your tail wags."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "To the delight of both you and the knight, the baker seems to have taken it upon herself to take charge of your kitchen after the knight recovered. You hope she stays forever."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The witch looks back and forth between the knight and the baker for a moment before bursting into sudden uncontrollable laughter. He only stops when the knight gives him a hard kick in the shin under the table. The baker turns very red, and her next laugh is much more forced."));
            textboxsender.DialogueTree.Add(new TextLine(Baker, 2, "Right... um, I have to get to work. I'll leave you guys to it. I'll be back for dinner!"));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "Your ears perk up."));
        }

        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The first of many conversations with the counselor is long and difficult on all sides except yours. You're just happy that the knight and the witch are going to be nicer to each other and be better about sharing."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "Of course, the knight and the witch still have a lot of work left to do, and you doubt the witch will give up his evil plans again so easily. But the three of you have an uneasy truce for now."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The three of you even have dinner together for the first time since you were a puppy! The knight and the witch don't threaten to kill each other either, although they both look tempted a couple times..."));

        if (questManager.questLines[1].quests[2].questCompleted) // if all baker questline is complete
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The baker joins you at the table as well. This time, the witch keeps his laughter to himself, mostly."));
            textboxsender.DialogueTree.Add(new TextLine(Witch, 1, "So tell me, baker, what are your intentions with the tin can?"));
            textboxsender.DialogueTree.Add(new TextLine(Baker, 2, "Um! Well..."));
            textboxsender.DialogueTree.Add(new TextLine(Knight, 0.8f, "Butt out, hag. My personal life is none of your business anymore."));
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "The witch finally lets out his laugh. You don't have no idea what's so funny, but you're very happy. You fall asleep on the knight's lap as three of your favourite people argue about the witch being nosy."));
        }
        else
        {
            textboxsender.DialogueTree.Add(new TextLine(null, 1, "You fall asleep on the knight's lap as two of your favourite people argue about something very stupid."));
        }

        textboxsender.DialogueTree.Add(new TextLine(null, 1, "The next day, the witch returns to the woods but now so do you. You stay for a week, helping around the forest, cuddling with the witch and learning magic."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "His minions aren't sure what to make of you, but you manage to convince them to throw a ball for you a couple of times. They eventually get used to the temporary truce."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "Then, the witch sneaks both of you back into town. You hide under long black cloaks and try to avoid being spotted; it's a lot of fun! Your wagging tail nearly exposes you both several times."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "You both join the knight for the now weekly counselling session at the knight's home. Then the witch, with a 'Papa loves you very much <3' and a quick kiss goodbye, the witch slinks back to the forest. You're already looking forward to seeing him again after your week with the knight."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "Before the knight was cursed, you used to sleep in your own bed most nights. Now, the knight lifts you into the air and then drops you onto her bed. You wrestle until you wear each other out then fall asleep tangled in the sheets. The last thing you hear is..."));
        textboxsender.DialogueTree.Add(new TextLine(Knight, 1, "I love you so much, my goodest boy. I've missed you."));
        textboxsender.DialogueTree.Add(new TextLine(null, 1, "True ending: Happyish Coparenting"));

        textboxsender.DialogueSequenceStarts();
        StartCoroutine(TextBoxCheck()); // add a corountine that waits until the textbox is inactive again before starting a new day with yield return new WaitUntil(() => bool true); but idk
    }
}