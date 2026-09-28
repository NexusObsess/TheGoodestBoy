
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class QuestSpawner : MonoBehaviour
{

    public QuestManager questManager;
    public GameManager gameManager;
    public GameObject[] rooms;
    [SerializeField] GameObject Witch;
    
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questManager = Object.FindFirstObjectByType<QuestManager>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
        StartCoroutine(FindRooms());

    }
    IEnumerator FindRooms()
    {
        
        yield return new WaitForSeconds(1);
        rooms = GameObject.FindGameObjectsWithTag("SpawnFloor");
        SpawnQuestItems();
        
    }

    void SpawnQuestItems()
    {
        if (gameManager.currentDay == 5)
        {
            GameObject questItem = Instantiate(Witch, new Vector3(4.5f, 0 , 0), Quaternion.identity);
        }
        foreach (Quest quest in questManager.activeQuests)
        {
           
            if (quest.QuestType == Quest.questType.Fetch)
            {
               
                for (int i = 0; i < quest.requiredItemAmount; i++)
                {
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                    Vector3 pos = randomRoom.transform.position;//Checks position of the room to spawn enemies within
                    float posX = pos.x;
                    float posY = pos.y;
                    
                   
                    GameObject questItem = Instantiate(quest.requiredItemPF, new Vector3(Random.Range(posX += 5, posX -= 5), Random.Range(posY += 5, posY -= 5), 0), Quaternion.identity);
                    questItem.SetActive(true);
                    Debug.Log(questItem + " Spawned");
                    
                    
                }
               
            }

            if (quest.QuestType == Quest.questType.Hunt)
            {
                for (int i = 0; i < quest.requiredEnemyAmount; i++)
                {
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                    Vector3 pos = randomRoom.transform.position;//Checks position of the room to spawn enemies within
                    float posX = pos.x;
                    float posY = pos.y;
                    GameObject questEnemy = Instantiate(quest.requiredEnemyPF, new Vector3(Random.Range(posX += 5, posX -= 5), Random.Range(posY += 5, posY -= 5), 0), Quaternion.identity);
                    questEnemy.SetActive(true);
                }

            }

            if (quest.QuestType == Quest.questType.Talk)
            {
                
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                    Vector3 pos = randomRoom.transform.position;//Checks position of the room to spawn enemies within
                    float posX = pos.x;
                    float posY = pos.y;
                    GameObject questNPC = Instantiate(quest.questNPCIDPF, new Vector3(Random.Range(posX += 5, posX -= 5), Random.Range(posY += 5, posY -= 5), 0), Quaternion.identity);
                    questNPC.SetActive(true);
               
            }

        }
    }
}
