using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class TextBoxSender : MonoBehaviour
{
    public List<TextLine> DialogueTree;
    int currentOnscreenLine = 0;
    int previousOnscreenLine = 0;
    TextBox textbox;
    GameObject textboxobject;

    BoxCollider2D[] colliders;

    void Start()
    {
        textbox = FindFirstObjectByType<TextBox>();
        textboxobject = GameObject.Find("TextBox");
    }

    public void DialogueSequenceStarts()
    {
        //Debug.Log("Function called");
        if (currentOnscreenLine == DialogueTree.Count) return;
        //Debug.Log("return check");

        textbox.ShowText(DialogueTree[currentOnscreenLine], this);
        currentOnscreenLine++;
    }

    public void NextLine(InputAction.CallbackContext context)
    {
        if (context.performed && DialogueTree.Count != 0)
        {
            Debug.Log("Pressed");
            if (currentOnscreenLine == DialogueTree.Count)
            {
                int tempIndex = currentOnscreenLine - 1;
                textbox.TextClear();
                textboxobject.SetActive(false);

                colliders = FindObjectsOfType<BoxCollider2D>(true);
                foreach (BoxCollider2D col in colliders)
                {
                    col.enabled = true;
                }

                DialogueTree = null;
                return;
            }

            previousOnscreenLine++;

            textbox.ShowText(DialogueTree[currentOnscreenLine], this);
            currentOnscreenLine++;
        }
    }

    public void GoBackLine()
    {
        currentOnscreenLine -= 1;
    }
}