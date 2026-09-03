using UnityEngine;
using System.Collections.Generic;
using TMPro;

//[System.Serializable]
public class NPC : MonoBehaviour
{
    [Header("Dialogue Customization")]
    public string Name;
    public float TextSpeed = 1;
    public Color TextColour;
    public AudioSource TextSound;
    public TMP_FontAsset TextFont;
    public int TextSize;

    public List<ConditionalDialogue> PossibleSequences;

    GameObject textboxobject;
    TextBoxSender textboxsender;
    BoxCollider2D[] colliders;

    GameManager gameManager;

    void Start()
    {
        textboxobject = GameObject.Find("TextBox");
        textboxsender = FindFirstObjectByType<TextBoxSender>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");

        textboxobject.SetActive(true);

        for (int i = 0; i < PossibleSequences.Count; i++)
        {
            if (gameManager.TownMorale >= PossibleSequences[i].townMoraleThreshold)
            {
                //Debug.Log(gameManager.TownMorale + " versus " + PossibleSequences[i].townMoraleThreshold);
                textboxsender.DialogueTree = PossibleSequences[i].DialogueTree;
                //Debug.Log("Matched conditions, given dialogue tree");
                break;
            }
        }

        textboxsender.DialogueSequenceStarts();

        colliders = BoxCollider2D.FindObjectsOfType<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            col.enabled = false;
        }
    }

    [System.Serializable]
    public class ConditionalDialogue
    {
        public List<TextLine> DialogueTree = new List<TextLine>();
        public float townMoraleThreshold;
    }
}