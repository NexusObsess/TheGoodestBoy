using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

//[System.Serializable]
public class NPC : MonoBehaviour // not system.serializable so npc dialogue can be triggered directly and to drag in drop them in as speaker in dialogue
{
    [Header("Dialogue Customization")]
    public string Name; // npc name
    public float TextSpeed = 1; // how fast npc talks
    public Color TextColour; // self explaintory
    public AudioClip TextSound; 
    public TMP_FontAsset TextFont;
    public int TextSize;

    PolygonCollider2D[] colliders;

    public TextTreeChooser DialogueVariations;
    
    TextBoxSender textboxsender;
    GameManager gameManager;

    Animator ExitAnimation;

    void Start()
    {
        textboxsender = FindFirstObjectByType<TextBoxSender>(); // finds the script that sends to actual textbox one by one based on player input
        gameManager = FindFirstObjectByType<GameManager>(); // finds game manager for townmorale int
        ExitAnimation = GameObject.Find("BlackOut").GetComponent<Animator>();
    }

    public void InteractedWith()
    {
        DialogueVariations.MoraleSelectCorrectTextTree();
        DialogueVariations.KnightHealthSelectCorrectTextTree();
        DialogueVariations.CurrentDaySelectCorrectTextTree();

        // disables colliders while npc dialogue is playing
        colliders = BoxCollider2D.FindObjectsOfType<PolygonCollider2D>();
        foreach (PolygonCollider2D col in colliders)
        {
            col.enabled = false;
        }

        textboxsender.DialogueSequenceStarts(); // would trigger the npc dialogue

        StartCoroutine(TextBoxCheck());
        // add a corountine that waits until the textbox is inactive again before starting a new day with yield return new WaitUntil(() => bool true); but idk
    }

    IEnumerator TextBoxCheck()
    {
        yield return new WaitUntil(() => !gameManager.TextActive);
        gameManager.textboxobject.SetActive(false);
        ExitAnimation.SetBool("Fade In", true);
        ExitAnimation.SetBool("Fade Out", false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        ExitAnimation.SetBool("Fade In", false);
        ExitAnimation.SetBool("Fade Out", true);
    }

    void OnMouseDown() // to replace when player interacting/talking in implemnted and if i have time to add npcs to town
    {
        InteractedWith();
    }
}