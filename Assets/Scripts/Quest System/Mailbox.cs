using System.Collections.Generic;
using System.Linq; // Required for LINQ
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MailBox : MonoBehaviour // still cooking
{
    public static MailBox current;

    [SerializeField] GameObject readQuestLetter;
    [SerializeField] GameObject DungeonGate;
    SpriteRenderer spriteRenderer;
    public GameObject panel;

    public List<GameObject> questLetters = new List<GameObject>(); // game objects that trigger the letter for the active quests
    public List<QuestLetter> QuestLetterSprite = new List<QuestLetter>();

    public GameObject exitLetter;
    public GameObject exitPanel;
    GameObject DefaultCurrentSelectedGameObject;
    GameObject PauseMenu;

    bool activated = false;

    [Header ("Sound Effects")]
    [SerializeField] AudioSource SoundEffect;
    [SerializeField] AudioClip PaperSE;
    [SerializeField] AudioClip MailBoxSE;

    [Header ("MailBox Sprites")]
    [SerializeField] Sprite NonRead;
    [SerializeField] Sprite Read;
    SpriteRenderer mailboxRenderer;

    Rigidbody2D PlayerRB;
    GameObject PlayerGO;

    void Awake()
    {
        if (current == null)
        {
            current = this;
            spriteRenderer = readQuestLetter.GetComponent<SpriteRenderer>();
            mailboxRenderer = GetComponent<SpriteRenderer>();
            mailboxRenderer.sprite = NonRead;
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public void ResetMailBox()
    {
        activated = false;
        mailboxRenderer.sprite = NonRead;
        DungeonGate.SetActive(true);
        QuestLetterSprite.Clear();
    }

    public void SetPopUp()
    {
        for (int i = 0; i < questLetters.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == questLetters[i])
            {
                PlayAudio(PaperSE);
                spriteRenderer.sprite = QuestLetterSprite[i].sprite;
                QuestLetterSprite[i].Read = true;
                readQuestLetter.SetActive(true);
                EventSystem.current.SetSelectedGameObject(exitLetter);
                panel.SetActive(false);
            }
        }

        bool allTrue = QuestLetterSprite.All(qls => qls.Read);

        if (allTrue)
        {
           exitPanel.SetActive(true);
           mailboxRenderer.sprite = Read;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enter");
            PlayerRB = other.gameObject.GetComponent<Rigidbody2D>();
            PlayerGO = other.gameObject;

            if (PlayerRB != null)
            {
                PlayerRB.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            }
            Debug.Log("Rigidbody position frozen due to collision.");
            Activate();

            if (!activated)
            {
                exitPanel.SetActive(false);
            // prevent player from moving
            }
            activated = true;
        }
    }

    public void PlayAudio(AudioClip ToPlay)
    {
        SoundEffect.clip = ToPlay;
        if (SoundEffect != null && SoundEffect.clip != null) // checks if audio source was on game object and if it has an audio clip attached
        {
            SoundEffect.Play();
        }
    }

    public void Activate()
    {
        PlayAudio(MailBoxSE);
        panel.SetActive(true);
        readQuestLetter.transform.position = PlayerGO.transform.position;

        DefaultCurrentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        DefaultCurrentSelectedGameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(questLetters[0]);
        PauseMenu = GameObject.Find("Pause");
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
    }

    public void Deactivate()
    {
        PlayAudio(MailBoxSE);

        PlayerRB.constraints = RigidbodyConstraints2D.None;
        PlayerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        panel.SetActive(false);
        DungeonGate.SetActive(false);

        DefaultCurrentSelectedGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(DefaultCurrentSelectedGameObject);
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(true);
        }
    }

    public void ExitLetter()
    {
        PlayAudio(PaperSE);
        readQuestLetter.SetActive(false);
        panel.SetActive(true);
        if (!exitPanel.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(questLetters[0]);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(exitPanel);
        }
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