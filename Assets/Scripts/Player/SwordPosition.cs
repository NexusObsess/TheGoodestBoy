using UnityEngine;
using UnityEngine.InputSystem;

public class SwordPosition : MonoBehaviour
{
    // Aim weapon towards mouse pointer
    public Vector2 rawPointerPos;
    public Vector2 pointerPosition { get; set; }

    GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
            if (gameManager.GameIsPaused) return;
            rawPointerPos = Input.mousePosition;
            pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = pointerPosition - (Vector2)transform.position;
            transform.right = lookDir;

        // Ensure a gamepad is connected
        if (Gamepad.current != null)
        {
            // Read the left stick as a Vector2 (X and Y values between -1 and 1)
            Vector2 stickInput = Gamepad.current.leftStick.ReadValue();

            // Log the direction vector
            if (stickInput.sqrMagnitude > 0.01f)
            {
                Debug.Log("Stick Direction: " + stickInput);
            }

        }


    }

}