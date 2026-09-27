using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public float healAmount = 1f;
    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            Debug.Log("Player Hit");
            collision.TryGetComponent<PlayerStats>(out PlayerStats pStats);
            pStats.HealPlayer(healAmount);
            Destroy(gameObject);
        }
    }
}
