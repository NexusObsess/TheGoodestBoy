using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;

    public float swordDamage = 1f;

    public void Start()
    {
        currentHealth = maxHealth;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player hit");
            float damage = collision.gameObject.GetComponent<Enemy>().damage;
            PlayerTakeDamage(damage);
        }
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
