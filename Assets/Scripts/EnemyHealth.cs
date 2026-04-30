using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50;

    [Header("Hit Feedback")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashTime = 0.15f;

    private int currentHealth;
    private int roomNumber;

    private Renderer enemyRenderer;
    private Color defaultColor;

    private bool isDead;
    private bool isRegistered;

    private void Awake()
    {
        currentHealth = maxHealth;

        enemyRenderer = GetComponent<Renderer>();

        if (enemyRenderer != null)
        {
            defaultColor = enemyRenderer.material.color;
        }
    }

    public void Initialize(int roomNumberFromManager)
    {
        roomNumber = roomNumberFromManager;
        isRegistered = true;

        Debug.Log(gameObject.name + " зарегистрирован как враг комнаты " + roomNumber);
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log(gameObject.name + " получил урон: " + damage);
        Debug.Log("Осталось здоровья: " + currentHealth);

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

        if (isRegistered && GameManager.Instance != null)
        {
            GameManager.Instance.EnemyKilled(roomNumber);
        }

        Debug.Log(gameObject.name + " уничтожен");

        Destroy(gameObject);
    }
}