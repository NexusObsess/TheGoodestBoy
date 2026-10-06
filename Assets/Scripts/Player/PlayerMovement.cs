using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public SpriteRenderer sRenderer;
    private bool facingRight = true;

    bool isSprinting;
    bool isKnockedBack;

    GameManager gameManager;

    //SORRY I WAS TRYIONG TO GET CONTROLLER TO WORK

    //InputSystem_Actions ISActions;
  

    //void Awake()
    //{
    //    ISActions = new InputSystem_Actions();
    //    ISActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
    //    ISActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    //}

    //void MoveForward()
    //{
    //    transform.localPosition *= moveSpeed;
    //}

    //void Update()
    //{
    //    Vector2 m = new Vector2(moveInput.x, moveInput.y) * Time.deltaTime;
    //    transform.Translate(m, Space.World);
    //}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerSpawn = spawnPoint.position;
        //transform.position = playerSpawn;

        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();

        speed = moveSpeed;
        isSprinting = false;
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("isSprinting", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            if (!gameManager.GameIsPaused) //OR attacking || attack
            {
                MoveDog();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void MoveDog()
    {
        if (moveInput != Vector2.zero)
        {
            if (isSprinting)
            {
                speed = moveSpeed * sprintSpeed;
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isSprinting", true);
                rb.linearVelocity = moveInput * speed;
            }
            else if (!isSprinting)
            {
                speed = moveSpeed;
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
                anim.SetBool("isSprinting", false);
                rb.linearVelocity = moveInput * speed;
            }
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isSprinting", false);
            rb.linearVelocity = Vector2.zero;
        }

        Flip();
    }

    void Flip()
    {
        if (moveInput.x > 0)
        {
            sRenderer.flipX = true;
        }
        else if (moveInput.x < 0)
        {
            sRenderer.flipX = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (gameManager.GameIsPaused) return;

        moveInput = context.ReadValue<Vector2>();
        


    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (gameManager.GameIsPaused) return;

        if (context.started)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }

       
    }

    public void Knockback(Transform enemy, float knockbackForce, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(StunTimer(stunTime));
    }


    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
        
    }
}
