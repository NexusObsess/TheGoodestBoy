using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class TextBoxSender : MonoBehaviour
{
    public List<TextLine> DialogueTree;
    int currentOnscreenLine = 0;
    int previousOnscreenLine = 0;
    [SerializeField] TextBox textbox;
    [SerializeField] GameObject textboxobject;

    BoxCollider2D[] colliders;

    public void DialogueSequenceStarts()
    {
        //Debug.Log("Function called");
        textboxobject.SetActive(true);
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
                if (textbox.typingCoroutine == null)
                {
                    textbox.TextClear();
                    textboxobject.SetActive(false);
                }
                else
                {
                    StopCoroutine(textbox.typingCoroutine);
                    textbox.mainText.text = DialogueTree[tempIndex].Line;

                    //textbox.typingCoroutine = null;
                }

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