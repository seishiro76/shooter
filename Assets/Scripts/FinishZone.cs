using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CheckFinish(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckFinish(other);
    }

    private void CheckFinish(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TryFinishLevel();
        }
        else
        {
            Debug.LogWarning("GameManager не найден. Невозможно проверить завершение уровня.");
        }
    }
}