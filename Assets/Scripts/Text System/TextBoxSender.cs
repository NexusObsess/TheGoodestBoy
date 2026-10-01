using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class TextBoxSender : MonoBehaviour
{
    public List<TextLine> DialogueTree; // LEAVE BLANK!! only public so it can be added to as needed
    int currentOnscreenLine = 0; // what line is on screen
    int previousOnscreenLine = 0; // for when clicking through fast to make all text appear immediately
    [SerializeField] TextBox textbox;
    [SerializeField] GameObject textboxobject;

    public AudioSource DogSoundEffectPlayer;

    BoxCollider2D[] colliders; // prevents player from clicking anything etc while the textbox is happening

    GameManager gamemanager;

    GameObject DefaultCurrentSelectedGameObject;

    // public Rigidbody2D PlayerRB;

    public void DialogueSequenceStarts() // called from other scripts
    {
        Debug.Log("Sequence starts");
        DefaultCurrentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        //Debug.Log("Function called");
        if (currentOnscreenLine == DialogueTree.Count) return; // if the dialogue tree is completed, don't continue
        Debug.Log("return check");

        gamemanager = FindFirstObjectByType<GameManager>();
        gamemanager.TextActive = true;
        gamemanager.GameIsPaused = true;

        textboxobject.SetActive(true); // textbox appears on screen
        textbox.ShowText(DialogueTree[currentOnscreenLine], this); // sends first line to text box
        currentOnscreenLine++; // 1

        // PlayerRB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
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
                    gamemanager.TextActive = false;
                    currentOnscreenLine = 0;
                    previousOnscreenLine = 0;

                    // PlayerRB.constraints = RigidbodyConstraints2D.None;
                    // PlayerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
                    gamemanager.GameIsPaused = false;

                    DialogueTree.Clear(); // empties the list for next text sequence

                    StartCoroutine(Reassign());
                }
                else // meaning if the player is just clicking through the text quickly, was being weird on last line otherwise
                {
                    textbox.TextStop();
                }

                return;
            }

            previousOnscreenLine++;

            textbox.ShowText(DialogueTree[currentOnscreenLine], this); // starts the text appearing
            currentOnscreenLine++;
        }
    }

    IEnumerator Reassign()
    {
        yield return new WaitForSeconds(1); // Waits 5 seconds (scaled)
        EventSystem.current.SetSelectedGameObject(DefaultCurrentSelectedGameObject);
    }

    public void GoBackLine() // called from textbox script
    {
        currentOnscreenLine -= 1;
    }
}