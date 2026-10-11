using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections;
public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    GameObject player;

    private Enemy enemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<Enemy>();
    }

    public void Knockback(float knockbackForce, float knockbackTime, float stunTime)
    {
        enemy.ChangeState(EnemyState.Knockback);
        StartCoroutine(StunTimer(knockbackTime, stunTime));
        Debug.Log("Knockback Applied");
        Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemy.ChangeState(EnemyState.Patrol);
    }
}
