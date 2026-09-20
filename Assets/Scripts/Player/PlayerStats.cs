using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;

    public float swordDamage = 1f;

    // Animation and Sword Reference
    [Header("Sword Reference")]
    private Animator anim;
    [SerializeField] GameObject swordObj;
    private SpriteRenderer sRenderer;

    public void Start()
    {
        currentHealth = maxHealth;
    }


    public void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Debug.Log("You died");
        }
    }



}
