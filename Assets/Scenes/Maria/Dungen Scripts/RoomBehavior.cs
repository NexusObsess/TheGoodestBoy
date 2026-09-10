using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class RoomBehavior : MonoBehaviour
{

    public GameObject[] walls; // 0 - up, 1 - down, 2 - right, 3 - left
    public GameObject[] doors;

    public bool[] testStatus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRoom(testStatus);
    }

    void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
