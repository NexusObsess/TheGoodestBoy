using UnityEngine;

public class Item : MonoBehaviour
{
    private Collider2D itemCollider;


    public string itemName;

    public float degreesPerSecond = 15f;
    public float amplitude = 0.1f;
    public float frequency = 1f;

    Vector3 posOffset;
    Vector3 tempPos;

    void Awake()
    {
        itemCollider = GetComponent<Collider2D>();
        posOffset = transform.position;
    }

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

            Debug.Log("You picked up " + itemName);

            // Add item to inventory(?)


            Destroy(gameObject);
        }
    }
}
