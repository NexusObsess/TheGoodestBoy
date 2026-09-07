using UnityEngine;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private GameObject spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = spawnPoint.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
