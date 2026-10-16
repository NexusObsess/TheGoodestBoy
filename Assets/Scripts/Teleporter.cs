using System.Collections;
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
    Animator ExitAnimation;
    

    void Start()
    {
        player = GameObject.Find("Doggie");
        bound = GameObject.Find("CameraBounds");
        gameManager = FindFirstObjectByType<GameManager>();
        timer = FindFirstObjectByType<Timer>();
        ExitAnimation = GameObject.Find("BlackOut").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsStartDoor && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teleporting To Dungeon");
            player.gameObject.transform.position = Dungeon.transform.position;
            timer.TimerStart();
            gameManager.MusicPlayer.clip = gameManager.Dungeon;
            gameManager.MusicPlayer.Play();
            StartCoroutine(FadeToBlack());
            
        }
        if (!IsStartDoor && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Teleporting To Town");
            player.gameObject.transform.position = Town.transform.position;
            gameManager.EndDay();
            timer.ResetTimer();
        }

    }
 // bound.gameObject.transform.position = Dungeon.transform.position;
    IEnumerator FadeToBlack()
    {
        ExitAnimation.SetBool("Fade In", true);
        ExitAnimation.SetBool("Fade Out", false);
        yield return new WaitForSeconds(0.5f);
        ExitAnimation.SetBool("Fade In", false);
        ExitAnimation.SetBool("Fade Out", true);
    }

    IEnumerator FadeOutBlack()
    {
        ExitAnimation.SetBool("Fade In", true);
        ExitAnimation.SetBool("Fade Out", false);
        yield return new WaitForSeconds(0.5f);
        ExitAnimation.SetBool("Fade In", false);
        ExitAnimation.SetBool("Fade Out", true);
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
