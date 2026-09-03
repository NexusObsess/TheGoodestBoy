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
    // public bool read = false;
}