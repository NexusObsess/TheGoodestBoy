using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTemp : MonoBehaviour // trying to figure stuff out
{
    [SerializeField] private float moveSpeed = 7f;

    private Rigidbody2D rb;
    private Vector2 input;

    GameObject MailBox;
    MailBox mailboxscript;

    bool canInteract = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        MailBox = GameObject.Find("Mailbox");
        mailboxscript = FindFirstObjectByType<MailBox>();
        MailBox.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mailbox"))
        {
            Debug.Log("Enter");
            canInteract = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            Debug.Log("Interact");
            mailboxscript.Activate();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mailbox"))
        {
            Debug.Log("Exit");
            canInteract = false;
        }
    }
}
