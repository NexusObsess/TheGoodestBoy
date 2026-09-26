using UnityEngine;

public class Interactable : MonoBehaviour // still cooking
{
    [SerializeField] GameObject readQuestLetter;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject letterBackground;
    Sprite letterPopUp;
    bool Selected = false;
    public GameObject panel;

    void Start()
    {
        spriteRenderer = readQuestLetter.GetComponent<SpriteRenderer>();
    }

    public void SetPopUp(Sprite popup)
    {
        // Debug.Log("Set pop up check");
        letterPopUp = popup;
    }
}