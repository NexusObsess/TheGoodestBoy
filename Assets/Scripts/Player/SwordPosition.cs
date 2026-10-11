using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class SwordPosition : MonoBehaviour
{
    // Aim weapon towards mouse pointer
    public Vector2 rawPointerPos;
    public Vector2 pointerPosition { get; set; }

    GameManager gameManager;
    public float moveSpeed;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
         if (gameManager.GameIsPaused) return;



        // Ensure a gamepad is connected
        if (Gamepad.current != null)
        {
            // Read the left stick as a Vector2 (X and Y values between -1 and 1)
            Vector2 stickInput = Gamepad.current.rightStick.ReadValue();
            Debug.DrawRay(transform.position, stickInput * 5f, Color.red);

            if (stickInput.sqrMagnitude > 0.1f) // Deadzone check
            {
                // Turn the stick direction into an angle directly
                float targetAngle = Mathf.Atan2(stickInput.y, stickInput.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), moveSpeed * Time.deltaTime);

                return;
            }
        }
        else
        {
            rawPointerPos = Input.mousePosition;
            pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = pointerPosition - (Vector2)transform.position;
            transform.right = lookDir;
        }



    }

}