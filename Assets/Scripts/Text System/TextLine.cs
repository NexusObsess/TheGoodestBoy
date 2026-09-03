using UnityEngine;

[System.Serializable]
public class TextLine
{
    [Header("Dialogue Customization")]
    public NPC Speaker;
    public int TextSpeed = 1;
    public string Line;
    public enum textEffect { Normal, Addon, Shaking, InstantHide, Instant, Choice }
    public textEffect TextEffect;

    public TextLine(NPC speaker, int textSpeed, string line)
    {
        Speaker = speaker;
        TextSpeed = textSpeed;
        Line = line;
    }
    // public bool read = false;
}