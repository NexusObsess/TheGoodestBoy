using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ConditionalDialogue
{
    public List<TextLine> DialogueTree = new List<TextLine>(); // list of the dialogue in a row
    public float townMoraleThreshold; // conditions for appearing, may add more
}