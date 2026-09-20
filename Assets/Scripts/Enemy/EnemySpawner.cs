using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmerPrefab;
    [SerializeField]
    private GameObject bigSwarmerPrefab;

    private float swarmerInterval = 3.5f;
    private float bigSwarmerInterval = 10f;
    [SerializeField] GameManager gameManager;
    private float MaxEnemies = 6;
    private int EnemiesSpawned;
    private int EnemiesSpawning;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxEnemies -= (gameManager.TownMorale /= 20);
       
        Debug.Log("Max Enemies = " + MaxEnemies);
        float randomFloat = Random.Range(0, MaxEnemies); 
        int roundValue = Mathf.RoundToInt(randomFloat);
        EnemiesSpawning = (int)randomFloat;
        Debug.Log("Enemies Spawning = " + EnemiesSpawning);
        StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));
        StartCoroutine(spawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-3f, 3f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        EnemiesSpawned++;
        if (EnemiesSpawned >= EnemiesSpawning)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(spawnEnemy(interval, enemy));
        }
        
    }
}
