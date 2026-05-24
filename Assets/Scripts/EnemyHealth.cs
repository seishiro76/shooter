using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action<EnemyHealth> Died;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50;

    [Header("Hit Feedback")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashTime = 0.15f;

    private int currentHealth;
    private Renderer enemyRenderer;
    private Color defaultColor;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        enemyRenderer = GetComponent<Renderer>();

        if (enemyRenderer != null)
        {
            defaultColor = enemyRenderer.material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (enemyRenderer != null)
        {
            StartCoroutine(HitFlashCoroutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlashCoroutine()
    {
        enemyRenderer.material.color = hitColor;

        yield return new WaitForSeconds(hitFlashTime);

        if (!isDead)
        {
            enemyRenderer.material.color = defaultColor;
        }
    }

    private void Die()
    {
        isDead = true;

        Died?.Invoke(this);

        Destroy(gameObject);
    }
}