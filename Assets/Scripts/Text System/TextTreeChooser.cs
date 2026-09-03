using UnityEngine;
using System.Collections.Generic;
using TMPro;

//[System.Serializable]
public class TextTreeChooser : MonoBehaviour
{
    public List<ConditionalDialogue> PossibleSequences;

    GameObject textboxobject;
    TextBoxSender textboxsender;
    BoxCollider2D[] colliders;

    GameManager gameManager;

    void Start()
    {
        // textboxobject = GameObject.Find("TextBox");
        textboxsender = FindFirstObjectByType<TextBoxSender>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void SelectCorrectTextTree()
    {
        // textboxobject = GameObject.Find("TextBox");
        textboxsender = FindFirstObjectByType<TextBoxSender>();
        // textboxobject.SetActive(true);

        for (int i = 0; i < PossibleSequences.Count; i++)
        {
            if (gameManager.TownMorale >= PossibleSequences[i].townMoraleThreshold)
            {
                textboxsender.DialogueTree.AddRange(PossibleSequences[i].DialogueTree);
                break;
            }
        }

        textboxsender.DialogueSequenceStarts();
    }
}