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

    bool isSprinting;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerSpawn = spawnPoint.position;
        //transform.position = playerSpawn;

        rb = GetComponent<Rigidbody2D>();
        speed = moveSpeed;
        isSprinting = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSprinting == true)
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
        moveInput = context.ReadValue<Vector2>();
    }

    public void Sprint()
    {

        isSprinting = true;
    }

}
