using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject defeatPanel; // панель "Поражение"
    [SerializeField] private GameObject winPanel;    // панель "Победа"

    private bool isShown;

    private void Awake()
    {
        // прячем обе панели на старте
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    // Повесить на GameManager -> onPlayerDied
    public void ShowDefeat()
    {
        if (isShown)
        {
            return;
        }

        isShown = true;

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }

        FreezeGame();
    }

    // Повесить на GameManager -> onLevelCompleted
    public void ShowWin()
    {
        if (isShown)
        {
            return;
        }

        isShown = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        FreezeGame();
    }

    // Кнопка "Начать заново" — перезапуск текущей сцены
    public void Restart()
    {
        Time.timeScale = 1f; // вернуть время перед перезагрузкой
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Кнопка "Выйти"
    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // в редакторе просто останавливаем Play
#else
        Application.Quit(); // в собранной игре закрываем приложение
#endif
    }

    private void FreezeGame()
    {
        Time.timeScale = 0f; // останавливаем игру (враги, движение замирают)

        Cursor.lockState = CursorLockMode.None; // освобождаем курсор
        Cursor.visible = true;
    }
}