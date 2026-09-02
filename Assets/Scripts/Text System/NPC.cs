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
}