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

    BoxCollider2D[] colliders;

    void OnMouseDown()
    {
        Debug.Log("Clicked");

        //textboxsender.DialogueSequenceStarts();

        colliders = BoxCollider2D.FindObjectsOfType<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            col.enabled = false;
        }
    }
}