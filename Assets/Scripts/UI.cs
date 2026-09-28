using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject UnPauseButton;
    [SerializeField] GameObject MailboxButton;
    [SerializeField] GameObject MailBoxUI;
    [SerializeField] GameObject XMailbox;



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
