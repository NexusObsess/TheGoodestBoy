using UnityEngine;

public class SwordPosition : MonoBehaviour
{
    // Aim weapon towards mouse pointer
    public Vector2 rawPointerPos;
    public Vector2 pointerPosition { get; set; }

    // Update is called once per frame
    private void Update()
    {

            rawPointerPos = Input.mousePosition;
            pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = pointerPosition - (Vector2)transform.position;
            transform.right = lookDir;

    }
}