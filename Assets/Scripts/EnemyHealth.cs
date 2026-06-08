using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action<EnemyHealth> Died;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50;

    [Header("Death Settings")]
    // 0 = уничтожить сразу (как у капсул). Для модели с анимацией смерти поставь ~2-2.5
    [SerializeField] private float destroyDelay = 0f;

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

        // Сообщаем подписчикам СРАЗУ (EnemyWave досчитает волну и откроет дверь без задержки).
        // EnemyAI поймает это же событие и запустит анимацию смерти.
        Died?.Invoke(this);

        // Уничтожаем с задержкой, чтобы анимация смерти успела проиграться.
        // При destroyDelay = 0 поведение идентично прежнему (мгновенное уничтожение).
        Destroy(gameObject, destroyDelay);
    }
}