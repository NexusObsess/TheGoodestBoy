using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 10f;
    public float currentHealth;

    public float swordDamage = 1f;

    // Animation and Sword Reference
    [Header("Sword Reference")]
    private Animator anim;
    public GameObject pSprite;
    private SpriteRenderer sRenderer;
    [SerializeField] GameObject swordObj;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;


    public void Start()
    {
        sRenderer = pSprite.GetComponent<SpriteRenderer>();
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
        StartCoroutine(Invulnerability());
        if (currentHealth <= 0f)
        {
            Debug.Log("You died");
        }
    }

    // Temporary invulnerability after taking damage
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            sRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            sRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
