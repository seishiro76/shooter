using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private bool isFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (isFinished)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        isFinished = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteLevel();
        }
    }
}