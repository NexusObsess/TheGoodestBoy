using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    QuestManager questManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questManager = FindFirstObjectByType<QuestManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
