using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Vector3 playerSpawn;
    public Transform spawnPoint;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 1.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpawn = spawnPoint.position;
        transform.position = playerSpawn;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Sprint()
    {
        
        //moveSpeed *= sprintSpeed;
    }

}
