
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class QuestSpawner : MonoBehaviour
{

    public QuestManager questManager;
    public GameManager gameManager;
    public GameObject[] rooms;
    [SerializeField] GameObject Witch;
    public static List<GameObject> QuestStuffs = new List<GameObject>();

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        StartCoroutine(FindRooms());

    }
    IEnumerator FindRooms()
    { //waits a second before collecting the spawning rooms 
        //this ensures they have spawned
        yield return new WaitForSeconds(1);
        rooms = GameObject.FindGameObjectsWithTag("SpawnFloor");
        SpawnQuestItems();
        
    }

    void SpawnQuestItems()
    {
        if (gameManager.currentDay == 5)
        { //spawns the witch on day five in the start room for dialogue
            GameObject witch = Instantiate(Witch, new Vector3(2f, -1f , 0), Quaternion.identity);
            QuestStuffs.Add(witch);
        }
        foreach (Quest quest in questManager.activeQuests)
        {//chooses a room then spawns a quest item in it. repeats for each item.
            if (quest.QuestType == Quest.questType.Fetch)
            {
                for (int i = 0; i < quest.requiredItemAmount; i++)
                { 
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                    Vector3 pos = randomRoom.transform.position;//Checks position of the room to spawn items within
                    float posX = pos.x;
                    float posY = pos.y;
                    GameObject questItem = Instantiate(quest.requiredItemPF, new Vector3(Random.Range(posX += 2, posX -= 2), Random.Range(posY += 1, posY -= 1), 0), Quaternion.identity);
                    questItem.SetActive(true);
                    QuestStuffs.Add(questItem);
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
                    GameObject questEnemy = Instantiate(quest.requiredEnemyPF, new Vector3(Random.Range(posX += 1, posX -= 1), Random.Range(posY += 0.4f, posY -= 0.4f), 0), Quaternion.identity);
                    questEnemy.SetActive(true);
                    QuestStuffs.Add(questEnemy);
                }

            }

            if (quest.QuestType == Quest.questType.Talk)
            {
                
                    GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                    Vector3 pos = randomRoom.transform.position;//Checks position of the room to spawn enemies within
                    float posX = pos.x;
                    float posY = pos.y;
                    GameObject questNPC = Instantiate(quest.questNPCIDPF, new Vector3(Random.Range(posX += 2, posX -= 2), Random.Range(posY += 1, posY -= 1), 0), Quaternion.identity);
                    questNPC.SetActive(true);
                    QuestStuffs.Add(questNPC);

            }

        }
    }
}
