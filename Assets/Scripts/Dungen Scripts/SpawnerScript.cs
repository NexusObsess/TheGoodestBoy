using Unity.VisualScripting;
using UnityEngine;


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
            GameObject newEnemy = Instantiate(swarmerPrefab, new Vector3(Random.Range(posX +=6, posX -=6), Random.Range(posY += 6, posY -= 6), 0), Quaternion.identity);
            newEnemy.SetActive(true);
         
            EnemiesSpawned++;
            SpawnEnemies();
        }
    }
}
