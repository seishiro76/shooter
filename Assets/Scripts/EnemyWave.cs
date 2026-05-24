using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    [Header("Wave Events")]
    [SerializeField] private UnityEvent onWaveCleared;

    private EnemyHealth[] enemies;
    private int enemiesAlive;
    private bool isCleared;

    private void Awake()
    {
        enemies = GetComponentsInChildren<EnemyHealth>();
        enemiesAlive = enemies.Length;

        foreach (EnemyHealth enemy in enemies)
        {
            enemy.Died += OnEnemyDied;
        }

        if (enemiesAlive == 0)
        {
            ClearWave();
        }
    }

    private void OnDestroy()
    {
        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Died -= OnEnemyDied;
            }
        }
    }

    private void OnEnemyDied(EnemyHealth enemy)
    {
        enemy.Died -= OnEnemyDied;
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            ClearWave();
        }
    }

    private void ClearWave()
    {
        if (isCleared)
        {
            return;
        }

        isCleared = true;
        onWaveCleared?.Invoke();
    }
}