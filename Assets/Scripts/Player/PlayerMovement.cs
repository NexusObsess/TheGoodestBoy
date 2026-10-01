using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Vector3 playerSpawn;
    public Transform spawnPoint;
    private float speed;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 1.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public Animator anim;

    bool isSprinting;

    GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerSpawn = spawnPoint.position;
        //transform.position = playerSpawn;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();

        speed = moveSpeed;
        isSprinting = false;
        anim.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSprinting == true && !gameManager.GameIsPaused)
        {
            speed = moveSpeed * sprintSpeed;
        }
        else
        {
            speed = moveSpeed;
        }
        rb.linearVelocity = moveInput * speed;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (gameManager.GameIsPaused) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void Sprint()
    {
        if (gameManager.GameIsPaused) return;
        isSprinting = true;
    }

}
