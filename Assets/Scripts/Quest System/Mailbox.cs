using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq; // Required for LINQ

public class MailBox : MonoBehaviour // still cooking
{
    [SerializeField] GameObject readQuestLetter;
    SpriteRenderer spriteRenderer;
    public GameObject panel;

    public List<GameObject> questLetters = new List<GameObject>(); // game objects that trigger the letter for the active quests
    public List<QuestLetter> QuestLetterSprite = new List<QuestLetter>();
    //public List<Sprite> QuestLetterSprite = new List<Sprite>();

    public GameObject exitLetter;
    public GameObject exitPanel;

    void Start()
    {
        spriteRenderer = readQuestLetter.GetComponent<SpriteRenderer>();
    }

    public void SetPopUp()
    {
        for (int i = 0; i < questLetters.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == questLetters[i])
            {
                // Debug.Log("Set pop up check");
                //spriteRenderer.sprite = QuestLetterSprite[i];
                spriteRenderer.sprite = QuestLetterSprite[i].sprite;
                QuestLetterSprite[i].Read = true;
                readQuestLetter.SetActive(true);
                EventSystem.current.SetSelectedGameObject(exitLetter);
            }
        }

        bool allTrue = QuestLetterSprite.All(qls => qls.Read);

        if (allTrue)
        {
           exitPanel.SetActive(true);
        }
    }

    public void Activate()
    {
        panel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(questLetters[0]);
        exitPanel.SetActive(false);
        Debug.Log(QuestLetterSprite.ToString());
    }

    public void Deactivate()
    {
        panel.SetActive(false);
    }

    public void ExitLetter()
    {
        readQuestLetter.SetActive(false);
        EventSystem.current.SetSelectedGameObject(questLetters[0]);
    }
}

[System.Serializable]
public class QuestLetter
{
    public Sprite sprite;
    public bool Read = false;
    
    public QuestLetter(Sprite spriteA, bool ReadA) // so you can add individual strings to textboxsender
    {
        sprite = spriteA;
        Read = ReadA;
    }
}