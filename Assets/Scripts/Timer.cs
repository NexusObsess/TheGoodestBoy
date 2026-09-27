using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerRunning = false;
    public TextMeshProUGUI timeText;
    [SerializeField] string GameOver;
    public GameManager gameManager;
    public bool TimePause;
    
    void Start()
    {
        GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
        timerRunning = true;
    }
    
    public void TimerStart()
    {
        timerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (timerRunning && TimePause)
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
