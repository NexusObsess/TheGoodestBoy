using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SpawnerScript : MonoBehaviour
{

    //QuestManager questManager;

    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;
    [SerializeField] private GameObject rangedPrefab;
    private GameManager gameManager;
    private float MaxEnemies = 6;
    private float MaxBigEnemies = 4;
    private float MaxRanged = 5;
    private int EnemiesSpawned = 0;
    private float EnemiesSpawning;
    private float BigSpawning;
    private float RangedSpawning;
    [SerializeField] string DungeonScene;
    public static List<GameObject> enemies = new List<GameObject>();
  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        Debug.Log("spawner working");
        gameManager = FindFirstObjectByType<GameManager>();

        //decides how many regular enemies
        EnemiesSpawning = Random.Range(1, MaxEnemies -= gameManager.TownMorale / 25);
        EnemiesSpawning = Mathf.RoundToInt(EnemiesSpawning);
        Debug.Log("Enemies Spawning = " + EnemiesSpawning);
        
        
        //decides how many big enemies. wont spawn until morale is below 70
        BigSpawning = Random.Range(0, MaxBigEnemies -= gameManager.TownMorale / 20);
        BigSpawning = Mathf.RoundToInt(BigSpawning);
        Debug.Log("Big Enemy Spawning = " + BigSpawning);

        //Decideds how many ranged. wont spawn until morale is below 90
        RangedSpawning = Random.Range(0, MaxRanged -= gameManager.TownMorale / 20);
        RangedSpawning = Mathf.RoundToInt(RangedSpawning);
        Debug.Log("Ranged Enemy Spawning = " + RangedSpawning);

        SpawnEnemies();

    }

   public void SpawnEnemies()
    {
        while (EnemiesSpawning > EnemiesSpawned)
        {
            Vector3 pos = this.transform.position;//Checks position of the room to spawn enemies within
            float posX = pos.x;
            float posY = pos.y;
            GameObject newEnemy = Instantiate(swarmerPrefab, new Vector3(Random.Range(posX +=1, posX -=1), Random.Range(posY += 1, posY -= 1), 0), Quaternion.identity);
            newEnemy.SetActive(true);
            enemies.Add(newEnemy);
            EnemiesSpawned++;

            int RangedSpawned = 0;
            int BigSpawned = 0;

            if (RangedSpawning > RangedSpawned)
            {
                GameObject newRanged = Instantiate(rangedPrefab, new Vector3(Random.Range(posX += 1, posX -= 1), Random.Range(posY += 1, posY -= 1), 0), Quaternion.identity);
                newRanged.SetActive(true);
                enemies.Add(newRanged);
                RangedSpawned++;
            }

            if (BigSpawning > BigSpawned)
            {
                GameObject newBig = Instantiate(bigSwarmerPrefab, new Vector3(Random.Range(posX += 1, posX -= 1), Random.Range(posY += 1, posY -= 1), 0), Quaternion.identity);
                newBig.SetActive(true);
                enemies.Add(newBig);
                BigSpawned++;
            }

            SpawnEnemies();
        }




    }

    public void RemoveEnemies()
    {
        foreach (var objects in enemies)
        {
            Destroy(objects);
        }
    }
}
