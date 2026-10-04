using Unity.VisualScripting;
using UnityEngine;


public class Teleporter : MonoBehaviour
{
    public GameObject player;
    public Transform Dungeon;
    public Transform Town;
    public bool IsStartDoor;
    public GameManager gameManager;
    public Timer timer;
    public GameObject bound;

    void Start()
    {
        player = GameObject.Find("Doggie");
        bound = GameObject.Find("CameraBounds");
        gameManager = FindFirstObjectByType<GameManager>();
        timer = FindFirstObjectByType<Timer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsStartDoor)
        {
            Debug.Log("Teleporting To Dungeon");
            player.gameObject.transform.position = Dungeon.transform.position;
            bound.gameObject.transform.position = Dungeon.transform.position;
            timer.TimerStart();
        }
        else
        {
            Debug.Log("Teleporting To Town");
            player.gameObject.transform.position = Town.transform.position;
            bound.gameObject.transform.position = Town.transform.position;
            gameManager.EndDay();
            gameManager.NewDay();
        }
        
    }



}
