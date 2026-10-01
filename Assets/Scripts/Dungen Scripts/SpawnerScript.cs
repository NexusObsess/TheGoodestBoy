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
    private GameManager gameManager;
    private float MaxEnemies = 6;
    private int EnemiesSpawned = 0;
    private int EnemiesSpawning;
    private float roomTopLeft;
    private float roomBottomRight;
    [SerializeField] string DungeonScene;
    public static List<GameObject> enemies = new List<GameObject>();
  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        


        Debug.Log("spawner working");
        gameManager = FindFirstObjectByType<GameManager>();   
        MaxEnemies -= gameManager.TownMorale / 25;

        Debug.Log("Max Enemies = " + MaxEnemies + gameObject.name);
        float randomFloat = Random.Range(1, MaxEnemies + 1);
        int roundValue = Mathf.RoundToInt(randomFloat);
        EnemiesSpawning = (int)randomFloat;
        Debug.Log("Enemies Spawning = " + EnemiesSpawning);
        SpawnEnemies();

    }

   public void SpawnEnemies()
    {
        while (EnemiesSpawning > EnemiesSpawned)
        {
            Vector3 pos = this.transform.position;//Checks position of the room to spawn enemies within
            float posX = pos.x;
            float posY = pos.y;
            GameObject newEnemy = Instantiate(swarmerPrefab, new Vector3(Random.Range(posX +=5, posX -=5), Random.Range(posY += 5, posY -= 5), 0), Quaternion.identity);
            newEnemy.SetActive(true);
            enemies.Add(newEnemy);
            EnemiesSpawned++;
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
