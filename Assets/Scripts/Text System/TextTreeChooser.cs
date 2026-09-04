using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextTreeChooser : MonoBehaviour
{
    public List<ConditionalDialogue> PossibleSequences; // list of system.serialized - check that script, should be arranged in descending order of townMoraleThreshold i.e., 90, 60, 30 etc

    TextBoxSender textboxsender;
    GameManager gameManager;

    void Start()
    {
        textboxsender = FindFirstObjectByType<TextBoxSender>(); // finds the script that sends to actual textbox one by one based on player input
        gameManager = FindFirstObjectByType<GameManager>(); // finds game manager for townmorale int
    }

    public void SelectCorrectTextTree()
    {
        textboxsender = FindFirstObjectByType<TextBoxSender>(); // again, just in case

        for (int i = 0; i < PossibleSequences.Count; i++) // repeats for each possible variation of the text tree
        {
            if (gameManager.TownMorale >= PossibleSequences[i].townMoraleThreshold) // if townmoracle is above or equal to the threshold set in editor
            {
                textboxsender.DialogueTree.AddRange(PossibleSequences[i].DialogueTree); // adds correct text tree to what is currently in the textbox sender
                break; // once they find the correct text tree to send, stops loop so multiple don't get sent
            }
        }

        textboxsender.DialogueSequenceStarts(); // may need to move?
    }
}