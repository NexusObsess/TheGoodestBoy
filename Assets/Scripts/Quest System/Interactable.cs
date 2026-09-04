using UnityEngine;

public class Interactable : MonoBehaviour // still cooking
{
    [SerializeField] GameObject readQuestLetter;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject letterBackground;
    Sprite letterPopUp;
    bool Selected = false;

    void Start()
    {
        spriteRenderer = readQuestLetter.GetComponent<SpriteRenderer>();
    }

    public void SetPopUp(Sprite popup)
    {
        Debug.Log("Set pop up check");
        letterPopUp = popup;
    }

    public void Highlighted()
    {
        Selected = true;
        transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        spriteRenderer.sprite = letterPopUp;
    }

        //     letterBackground.SetActive(true);
        // readQuestLetter.SetActive(true);
}