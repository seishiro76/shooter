using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Room 1 Door")]
    [SerializeField] private GameObject room1DoorBlocker;

    private int room1EnemiesAlive;
    private int totalEnemiesAlive;

    private bool room1Cleared;
    private bool levelCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CountEnemiesOnScene();

        Debug.Log("GameManager запущен");
        Debug.Log("Врагов в первой комнате: " + room1EnemiesAlive);
        Debug.Log("Всего врагов на уровне: " + totalEnemiesAlive);
    }

    private void CountEnemiesOnScene()
    {
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        totalEnemiesAlive = enemies.Length;
        room1EnemiesAlive = 0;

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy.RoomNumber == 1)
            {
                room1EnemiesAlive++;
            }
        }
    }

    public void EnemyKilled(int roomNumber)
    {
        totalEnemiesAlive = Mathf.Max(0, totalEnemiesAlive - 1);

        if (roomNumber == 1)
        {
            room1EnemiesAlive = Mathf.Max(0, room1EnemiesAlive - 1);

            Debug.Log("Осталось врагов в первой комнате: " + room1EnemiesAlive);

            if (room1EnemiesAlive <= 0 && !room1Cleared)
            {
                OpenRoom1Door();
            }
        }

        Debug.Log("Враг уничтожен. Осталось всего врагов: " + totalEnemiesAlive);
    }

    private void OpenRoom1Door()
    {
        room1Cleared = true;

        if (room1DoorBlocker != null)
        {
            room1DoorBlocker.SetActive(false);
            Debug.Log("Первая комната зачищена. Дверь открыта.");
        }
        else
        {
            Debug.LogWarning("Дверь первой комнаты не назначена в GameManager.");
        }
    }

    public bool CanFinishLevel()
    {
        return totalEnemiesAlive <= 0;
    }

    public void TryFinishLevel()
    {
        if (levelCompleted)
        {
            return;
        }

        if (CanFinishLevel())
        {
            levelCompleted = true;
            Debug.Log("Уровень завершён!");
        }
        else
        {
            Debug.Log("Нельзя завершить уровень. Сначала уничтожьте всех противников.");
        }
    }
}