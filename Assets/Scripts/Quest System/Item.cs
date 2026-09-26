using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    public float degreesPerSecond = 15f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    Vector3 posOffset;
    Vector3 tempPos;

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}
