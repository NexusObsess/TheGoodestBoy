using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    GameManager gameManager;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        
        //if (timer > 2)
        //{
        //    timer = 0;
        //    Shoot();
        //}
    }

    public void Shoot()
    {
        if (gameManager.GameIsPaused) return;
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
