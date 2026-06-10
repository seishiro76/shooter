using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private GameObject pausePanel; // панель "Пауза"

    private bool isPaused;

    private void Awake()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (inputHandler != null && inputHandler.EscapePressed)
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f; // замораживаем игру

        Cursor.lockState = CursorLockMode.None; // показываем курсор
        Cursor.visible = true;
    }

    // Кнопка "Продолжить"
    public void Resume()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f; // возвращаем время

        Cursor.lockState = CursorLockMode.Locked; // снова прячем курсор для обзора
        Cursor.visible = false;
    }

    // Кнопка "Выйти"
    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}