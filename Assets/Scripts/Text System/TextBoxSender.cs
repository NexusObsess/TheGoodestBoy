using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class TextBoxSender : MonoBehaviour
{
    public List<TextLine> DialogueTree; // LEAVE BLANK!! only public so it can be added to as needed
    int currentOnscreenLine = 0; // what line is on screen
    int previousOnscreenLine = 0; // for when clicking through fast to make all text appear immediately
    [SerializeField] TextBox textbox;
    [SerializeField] GameObject textboxobject;

    BoxCollider2D[] colliders; // prevents player from clicking anything etc while the textbox is happening

    GameManager gamemanager;

    public void DialogueSequenceStarts() // called from other scripts
    {
        //Debug.Log("Function called");
        if (currentOnscreenLine == DialogueTree.Count) return; // if the dialogue tree is completed, don't continue
        //Debug.Log("return check");

        gamemanager = FindFirstObjectByType<GameManager>();
        gamemanager.TextActive = true;

        textboxobject.SetActive(true); // textbox appears on screen
        textbox.ShowText(DialogueTree[currentOnscreenLine], this); // sends first line to text box
        currentOnscreenLine++; // 1
    }

    public void NextLine(InputAction.CallbackContext context) // called when player pushed next line button on the play
    {
        gamemanager = FindFirstObjectByType<GameManager>();
        if (DialogueTree.Count == 0)
        {
            if (!textboxobject.activeSelf) return;
            Debug.Log("NULL");
            gamemanager.TextActive = false;
            textbox.TextClear(); // clears text

            currentOnscreenLine = 0;
            previousOnscreenLine = 0;
            return;
        }

        if (context.performed && DialogueTree.Count != 0 && gamemanager.TextActive) // if dialogue tree is not empty
        {
            if (currentOnscreenLine == DialogueTree.Count) // if the dialogue tree is finished
            {
                //int tempIndex = currentOnscreenLine - 1; // for when clicking through fast to make all text appear immediately
                if (textbox.typingCoroutine == null) // if the textbox is still typing
                {
                    Debug.Log("null check");
                    textbox.TextClear(); // clears text
                    // textboxobject.SetActive(false); // deactivates the textbox
                    gamemanager.TextActive = false;
                    currentOnscreenLine = 0;
                    previousOnscreenLine = 0;

                    colliders = FindObjectsOfType<BoxCollider2D>(true); // gets all colliders
                    foreach (BoxCollider2D col in colliders)
                    {
                        col.enabled = true; // reenable
                    }

                    DialogueTree.Clear(); // empties the list for next text sequence
                }
                else // meaning if the player is just clicking through the text quickly, was being weird on last line otherwise
                {
                    // StopCoroutine(textbox.typingCoroutine); // stops corountine, may replace?
                    //GoBackLine(); // goes back line so the textbox sender doesn't skip the next line
                    textbox.TextStop();
                    //textbox.mainText.text = DialogueTree[tempIndex].Line; // show last line fully

                    //textbox.typingCoroutine = null;
                    // return;
                }

                return;
            }

            previousOnscreenLine++;

            textbox.ShowText(DialogueTree[currentOnscreenLine], this); // starts the text appearing
            currentOnscreenLine++;
        }
    }

    public void GoBackLine() // called from textbox script
    {
        currentOnscreenLine -= 1;
    }
}