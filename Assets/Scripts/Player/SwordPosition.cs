using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    // Aim weapon towards mouse pointer
    public Vector2 rawPointerPos;
    public Vector2 pointerPosition { get; set; }
    [SerializeField] GameManager gameManager;

    // Update is called once per frame
    private void Update()
    {
        //// Only aim if game has started
        //if (gameManager.gameStarted == true)
        //{
            rawPointerPos = Input.mousePosition;
            pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 lookDir = pointerPosition - (Vector2)transform.position;
            transform.right = lookDir;
        //}
        //else
        //{
        //    return;
        //}
    }
}