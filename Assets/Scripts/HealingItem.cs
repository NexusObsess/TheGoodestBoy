using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public float healAmount = 1f;
    public GameObject player;

    public float degreesPerSecond = 15f;
    public float amplitude = 0.1f;
    public float frequency = 1f;

    Vector3 posOffset;
    Vector3 tempPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
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
