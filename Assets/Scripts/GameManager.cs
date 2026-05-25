using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Events")]
    [SerializeField] private UnityEvent onLevelCompleted;
    [SerializeField] private UnityEvent onPlayerDied;

    private bool levelCompleted;
    private bool playerDied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CompleteLevel()
    {
        if (levelCompleted || playerDied)
        {
            return;
        }

        levelCompleted = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.LevelComplete);
        }

        onLevelCompleted?.Invoke();
    }

    public void PlayerDied()
    {
        if (playerDied || levelCompleted)
        {
            return;
        }

        playerDied = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SoundType.PlayerDeath);
        }

        onPlayerDied?.Invoke();
    }
}