using UnityEngine;
using System.Collections.Generic;
using TMPro;

//[System.Serializable]
public class NPC : MonoBehaviour // not system.serializable so npc dialogue can be triggered directly and to drag in drop them in as speaker in dialogue
{
    [Header("Dialogue Customization")]
    public string Name; // npc name
    public float TextSpeed = 1; // how fast npc talks
    public Color TextColour; // self explaintory
    public AudioSource TextSound; 
    public TMP_FontAsset TextFont;
    public int TextSize;

    BoxCollider2D[] colliders;

    void OnMouseDown() // to replace when player interacting/talking in implemnted and if i have time to add npcs to town
    {
        Debug.Log("Clicked");

        //textboxsender.DialogueSequenceStarts(); // would trigger the npc dialogue

        // disables colliders while npc dialogue is playing
        colliders = BoxCollider2D.FindObjectsOfType<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            col.enabled = false;
        }
    }
}