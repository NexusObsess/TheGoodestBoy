using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConditionalDialogue
{
    public List<TextLine> DialogueTree = new List<TextLine>();
    public float townMoraleThreshold;
}