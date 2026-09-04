using UnityEngine;

[System.Serializable]
public class TextLine
{
    [Header("Dialogue Customization")]
    public NPC Speaker; // who is talking
    public int TextSpeed = 1; // how fast the text appears, times by the npcs text speed in the textbox (more flexiable)
    public string Line; // actual text
    public enum textEffect { Normal, Addon, Shaking, InstantHide, Instant, Choice } // not currently implemented, may be added as needed
    public textEffect TextEffect;

    public TextLine(NPC speaker, int textSpeed, string line) // so you can add individual strings to textboxsender
    {
        Speaker = speaker;
        TextSpeed = textSpeed;
        Line = line;
    }
    // i.e., QuestResults.DialogueTree.Add(new TextLine(speaker, speed, string));
}