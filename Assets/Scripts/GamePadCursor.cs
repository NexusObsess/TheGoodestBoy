using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
public class GamePadCursor : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform canvasRectTransform;

    public float cursorSpeed = 1000f;

    private bool previousCursorState;
    private Mouse vMouse;

    //Camera must be tagged as "MainCamera" in the scene for this to work
    private Camera mainCamera;

    ////Check if Gamepad is connected
    //void Update()
    //{
    //    if (Gamepad.current != null)
    //    {
    //        Debug.Log("Gamepad detected: " + Gamepad.current.displayName);
    //    }
    //    else
    //    {
    //        Debug.Log("No gamepad detected.");
    //    }
    //}

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        if (vMouse == null)
        {
            vMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if(!vMouse.added)
        {
            InputSystem.AddDevice(vMouse);
        }

        // Pair the virtual mouse with the player input
        InputUser.PerformPairingWithDevice(vMouse, playerInput.user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(vMouse.position, position);
        }


        InputSystem.onAfterUpdate += UpdateMotion;
    }

    // Unpair the virtual mouse when the script is disabled
    private void OnDisable()
    {
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    // Update the virtual mouse position based on the gamepad input
    private void UpdateMotion()
    {
        if(vMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue *= cursorSpeed * Time.unscaledDeltaTime;
        Vector2 currentPosition = vMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(vMouse.position, newPosition);
        InputState.Change(vMouse.delta, deltaValue);

        bool isPressed = Gamepad.current.aButton.isPressed;
        if (previousCursorState != isPressed)
        {
            vMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, isPressed);
            InputState.Change(vMouse, mouseState);
            previousCursorState = isPressed;
        }

        AnchorCursor(newPosition);

    }

    // Anchors the cursor to the canvas based on the screen position
    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }
}
