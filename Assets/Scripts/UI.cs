using UnityEngine;

public class UI : MonoBehaviour
{

    public GameManager gameManager;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject UnPauseButton;
    [SerializeField] GameObject MailboxButton;
    [SerializeField] GameObject MailBoxUI;
    [SerializeField] GameObject XMailbox;


    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        //gameManager.NewDay();
    }
    public void Pause()
    {
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
        UnPauseButton.SetActive(true);
    }
    public void UnPause()
    {
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
        UnPauseButton.SetActive(false);
    }

    public void Mailbox()
    {
        MailboxButton.SetActive(false);
        MailBoxUI.SetActive(true);
       
    }
}
