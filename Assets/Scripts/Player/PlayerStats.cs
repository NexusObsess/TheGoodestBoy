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


    public void Start()
    {
        currentHealth = maxHealth;
        swordObj.SetActive(false);
    }


    public void Update()
    {
        // Attack Input
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }


    }



    // Activate Sword Attack
    void Attack()
    {
        swordObj.SetActive(true);
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
