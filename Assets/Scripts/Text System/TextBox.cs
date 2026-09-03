using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText; // the text game object itself
    [SerializeField] TextMeshProUGUI characterText;
    private Coroutine typingCoroutine; // Coroutine that types the string over the course of a few seconds
    string LastestLine;

    public void ShowText(TextLine NextLine, TextBoxSender textsequence) // function triggered through the other scripts passing through the string in brackets
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            mainText.text = LastestLine;

            typingCoroutine = null;
            textsequence.GoBackLine();
            return;
        }

        LastestLine = NextLine.Line;

        mainText.font = NextLine.Speaker.TextFont;
        mainText.color = NextLine.Speaker.TextColour;
        mainText.fontSize = NextLine.Speaker.TextSize;

        characterText.text = NextLine.Speaker.Name;
        characterText.font = NextLine.Speaker.TextFont;
        characterText.color = NextLine.Speaker.TextColour;
        characterText.fontSize = NextLine.Speaker.TextSize;

        TextClear();
        typingCoroutine = StartCoroutine(WriteText(NextLine)); // carries the string into the coroutine
    }

    private IEnumerator WriteText(TextLine NextLine)
    {
        for (int i = 0; i < NextLine.Line.Length; i++) // loop starts at the first letter (i) of the string, continues for all of the letters in the strings, moving on to the next letter
        {
            mainText.text += NextLine.Line[i]; // adds whatever current letter of the string to what is already in the text box
            //PlayAudio(); // function for audio

            float textSpeed = NextLine.TextSpeed * NextLine.Speaker.TextSpeed;
            textSpeed = textSpeed * 0.03f;
            yield return new WaitForSeconds(textSpeed); // wait a couple frames; why a corountine not normal function
        }
        
        typingCoroutine = null;
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
        }
        mainText.text = "";
    }
}
