using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Teleporter : MonoBehaviour
{
    public GameObject player;
    public Transform Dungeon;
    public Transform Town;
    public bool IsStartDoor;
    public GameManager gameManager;
    public Timer timer;
    public GameObject bound;
    public GameObject tutorial;
    private bool tutorialShowing = false;

    void Start()
    {
        player = GameObject.Find("Doggie");
        bound = GameObject.Find("CameraBounds");
        gameManager = FindFirstObjectByType<GameManager>();
        timer = FindFirstObjectByType<Timer>();
        //tutorial.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsStartDoor && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teleporting To Dungeon");
            player.gameObject.transform.position = Dungeon.transform.position;
            bound.gameObject.transform.position = Dungeon.transform.position;
            timer.TimerStart();

            gameManager.MusicPlayer.clip = gameManager.Dungeon;
            gameManager.MusicPlayer.Play();
            //ShowTutorial();
            
        }
        if (!IsStartDoor && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teleporting To Town");
            player.gameObject.transform.position = Town.transform.position;
            bound.gameObject.transform.position = Town.transform.position;
            gameManager.EndDay();
            timer.ResetTimer();
        }

    }

    //void ShowTutorial()
    //{
    //    tutorial.SetActive(true);
    //    tutorialShowing = true;
    //    gameManager.Pause();
    //}

    //void UnShowTutorial()
    //{
    //    tutorial.SetActive(false);
    //    tutorialShowing = false;
    //    gameManager.UnPause();
    //}

    //public void TutorialBind(InputAction.CallbackContext context)
    //{
    //    if (context.performed && tutorialShowing)
    //    {
    //        UnShowTutorial();
    //    }
    //}

}
