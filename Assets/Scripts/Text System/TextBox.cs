using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI mainText; // the text game object itself
    [SerializeField] TextMeshProUGUI characterText; // text of speaking character
    [SerializeField] GameObject characterTextBG; // only so if no speaker is set, so i can be removed
    public Coroutine typingCoroutine = null; // Coroutine that types the string over the course of a few seconds
    string LastestLine; // for when player is clicking through text fast

    public void ShowText(TextLine NextLine, TextBoxSender textsequence) // function triggered through the other scripts passing through the string in brackets
    {
        if (typingCoroutine != null) // if the typing coroutine is currently going, for when the player is clicking through text very fast
        {
            StopCoroutine(typingCoroutine); // stops it
            typingCoroutine = null;
            mainText.text = LastestLine; // straight up shows lastest line

            textsequence.GoBackLine(); // goes back line so the textbox sender doesn't skip the next line
            return; // doesn't continue
        }

        LastestLine = NextLine.Line; // for above check

        if (NextLine.Speaker != null) // if there is a speaker assigned to dialogue, meaning a npc is saying it
        {
            characterTextBG.SetActive(true); // character text part is visable

            // customizes the text based on the speaker's assigned traits
            mainText.font = NextLine.Speaker.TextFont;
            mainText.color = NextLine.Speaker.TextColour;
            mainText.fontSize = NextLine.Speaker.TextSize;

            characterText.text = NextLine.Speaker.Name;
            characterText.font = NextLine.Speaker.TextFont;
            characterText.color = NextLine.Speaker.TextColour;
            characterText.fontSize = NextLine.Speaker.TextSize;
        }
        else // if there is no speaker assigned to dialogue, meaning narrator dialogue
        {
            characterTextBG.SetActive(false); // character text part is NOT visable
            // returning to default
        }

        TextClear(); // clear text in the textbox
        typingCoroutine = StartCoroutine(WriteText(NextLine)); // carries the string into the coroutine
    }

    private IEnumerator WriteText(TextLine NextLine)
    {
        for (int i = 0; i < NextLine.Line.Length; i++) // loop starts at the first letter (i) of the string, continues for all of the letters in the strings, moving on to the next letter
        {
            mainText.text += NextLine.Line[i]; // adds whatever current letter of the string to what is already in the text box
            //PlayAudio(); // function for audio, not currently implemented

            float textSpeed = NextLine.TextSpeed; // base text speed from the line
            if (NextLine.Speaker != null) // speaker assigned check so text speed doesn't glitch
            {
                textSpeed = textSpeed * NextLine.Speaker.TextSpeed;
            }

            textSpeed = textSpeed * 0.03f; // keeps text quick
            yield return new WaitForSeconds(textSpeed); // wait a couple frames; why a corountine not normal function
        }
        
        typingCoroutine = null; // when the line is fully typed, coroutine is null for checks in other functions
    }

    // public void PlayAudio()
    // {
    //     if (source != null && source.clip != null) // checks if audio source was on game object and if it has an audio clip attached
    //     {
    //         source.Play();
    //     }
    //     else // debugging
    //     {
    //         Debug.LogWarning("No audio source on game object");
    //     }
    // }

    public void TextClear()
    {
        if (typingCoroutine != null) // Stop ongoing coroutines
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        mainText.text = ""; // removes what currently in text box
    }
}
