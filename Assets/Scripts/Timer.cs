using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 32;
    public bool timerRunning = false;
    public TextMeshProUGUI timeText;
    [SerializeField] string GameOver;
    public GameManager gameManager;
    public bool TimePause;
    [SerializeField] GameObject ThirtySeconds;
    private bool thirtyDone = false;
    
    void Start()
    {
        ThirtySeconds.SetActive(false);
        gameManager = Object.FindFirstObjectByType<GameManager>();
        timerRunning = true;
    }
    
    public void TimerStart()
    {
        timerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining <= 30 && !thirtyDone)
        {
            thirtyDone = true;
            StartCoroutine(Thirty());
            Debug.Log("Thirty Seconds Left");
        }
        if (timerRunning && gameManager.TextActive)
        {
            
            if (timeRemaining > 0)
            {
                timerRunning = false;
                DisplayTime(timeRemaining);
                
            }

        }

        else
        {
            timerRunning = true;
            Debug.Log(timerRunning);

            if (timeRemaining > 0)
            {

                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

            }
            else
            {
                Debug.Log("Day Failed - Time Ran Out");
                timeRemaining = 0;
                timerRunning = false;
                TimeOut();
            }

        }
    }
    IEnumerator Thirty()
    {
        ThirtySeconds.SetActive(true);
        yield return new WaitForSeconds(2);
        ThirtySeconds.SetActive(false);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeOut()
    {
        SceneManager.LoadScene(GameOver);
    }
}
