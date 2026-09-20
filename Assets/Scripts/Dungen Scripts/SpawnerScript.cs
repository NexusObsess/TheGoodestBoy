using Unity.VisualScripting;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //QuestManager questManager;

    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;
    GameManager gameManager;
    private float MaxEnemies = 6;
    private int EnemiesSpawned;
    private int EnemiesSpawning;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Debug.Log("spawner working");
        gameManager = FindFirstObjectByType<GameManager>();   
        MaxEnemies -= (gameManager.TownMorale /= 20);

        Debug.Log("Max Enemies = " + MaxEnemies);
        float randomFloat = Random.Range(0, MaxEnemies);
        int roundValue = Mathf.RoundToInt(randomFloat);
        EnemiesSpawning = (int)randomFloat;
        Debug.Log("Enemies Spawning = " + EnemiesSpawning);
        SpawnEnemies();

    }

   public void SpawnEnemies()
    {
        while (EnemiesSpawning < EnemiesSpawned)
        {
            Instantiate(swarmerPrefab);
            EnemiesSpawned++;
        }
    }
}
