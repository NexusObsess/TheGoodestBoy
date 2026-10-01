using Unity.VisualScripting;
using UnityEngine;


public class Teleporter : MonoBehaviour
{
    public GameObject player;
    public Transform Dungeon;
    public Transform Town;
    public bool IsStartDoor;
    public GameManager gameManager;

    void Start()
    {
        player = GameObject.Find("Doggie");
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsStartDoor)
        {
            Debug.Log("Teleporting To Dungeon");
            player.gameObject.transform.position = Dungeon.transform.position;
        }
        else
        {
            Debug.Log("Teleporting To Town");
            player.gameObject.transform.position = Town.transform.position;
            gameManager.EndDay();
        }
        
    }



}
