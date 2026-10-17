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
    private float MaxRanged = 2;
    private int EnemiesSpawned = 0;
    private int BigSpawned = 0;
    private int RangedSpawned = 0;
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
        if (BigSpawning < 0) { BigSpawning = 0; }
        Debug.Log("Big Enemy Spawning = " + BigSpawning);

        //Decideds how many ranged. wont spawn until morale is below 90
        RangedSpawning = Random.Range(0, MaxRanged -= gameManager.TownMorale / 60);
        RangedSpawning = Mathf.RoundToInt(RangedSpawning);
        if (RangedSpawning < 0) { RangedSpawning = 0; }
        Debug.Log("Ranged Enemy Spawning = " + RangedSpawning);

        SpawnEnemies();
        SpawnRanged();
        SpawnBig();

    }

   public void SpawnEnemies()
    {

        while (EnemiesSpawning > EnemiesSpawned)//spawns the enemies
        {
            Vector3 pos = this.transform.position;//Checks position of the room to spawn enemies within
            float posX = pos.x;
            float posY = pos.y;
            GameObject newEnemy = Instantiate(swarmerPrefab, new Vector3(Random.Range(posX +=1, posX -=1), Random.Range(posY += 0.4f, posY -= 0.4f), 0), Quaternion.identity);
            newEnemy.SetActive(true);
            enemies.Add(newEnemy);
            EnemiesSpawned++;

            SpawnEnemies();
        }
    }
    
    public void SpawnRanged()
    {
        
        while (RangedSpawning > RangedSpawned)//spawns the enemies
        {
            Vector3 pos = this.transform.position;//Checks position of the room to spawn enemies within
            float posX = pos.x;
            float posY = pos.y;
            GameObject newEnemy = Instantiate(rangedPrefab, new Vector3(Random.Range(posX += 1, posX -= 1), Random.Range(posY += 0.4f, posY -= 0.4f), 0), Quaternion.identity);
            newEnemy.SetActive(true);
            enemies.Add(newEnemy);
            RangedSpawned++;

            SpawnRanged();
        }
    }
    
    public void SpawnBig()
    {
        
        while (BigSpawning > BigSpawned)//spawns the enemies
        {
            Vector3 pos = this.transform.position;//Checks position of the room to spawn enemies within
            float posX = pos.x;
            float posY = pos.y;
            GameObject newEnemy = Instantiate(bigSwarmerPrefab, new Vector3(Random.Range(posX += 1, posX -= 1), Random.Range(posY += 0.4f, posY -= 0.4f), 0), Quaternion.identity);
            newEnemy.SetActive(true);
            enemies.Add(newEnemy);
            BigSpawned++;

            SpawnBig();
        }
    }

    public void RemoveEnemies()
    {//takes the enmies in the list and removes them at the end of the day
        //the leftover enemies in the list -1 from morale
        foreach (var objects in enemies)
        {
            enemies.Remove(objects);
            Destroy(objects);
        }
    }
}
