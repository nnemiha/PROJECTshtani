using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [Header("UI Panels")]
    public GameObject pauseMenuContainer;  // Панель меню паузы (Container)
    public GameObject optionsContainer;    // Панель настроек (Panel)

    void Start()
    {
        // При старте всё скрыто
        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(false);

        if (optionsContainer != null)
            optionsContainer.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если настройки открыты - закрываем их
            if (optionsContainer != null && optionsContainer.activeSelf)
            {
                CloseOptions();
            }
            // Если пауза открыта - закрываем её
            else if (pauseMenuContainer != null && pauseMenuContainer.activeSelf)
            {
                ResumeButton();
            }
            // Если ничего не открыто - открываем паузу
            else
            {
                OpenPauseMenu();
            }
        }
    }

    // Открыть меню паузы
    void OpenPauseMenu()
    {
        if (pauseMenuContainer != null)
        {
            pauseMenuContainer.SetActive(true);
            Time.timeScale = 0;

            Debug.Log("[PauseMenu] Пауза открыта");
        }
    }

    // Продолжить игру
    public void ResumeButton()
    {
        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(false);

        if (optionsContainer != null)
            optionsContainer.SetActive(false);

        Time.timeScale = 1;

        Debug.Log("[PauseMenu] Игра продолжена");
    }

    // Открыть настройки
    public void OpenOptions()
    {
        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(false);

        if (optionsContainer != null)
            optionsContainer.SetActive(true);

        // Время остаётся остановленным
        Debug.Log("[PauseMenu] Настройки открыты");
    }

    // Закрыть настройки (вернуться в меню паузы)
    public void CloseOptions()
    {
        if (optionsContainer != null)
            optionsContainer.SetActive(false);

        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(true);

        Debug.Log("[PauseMenu] Настройки закрыты");
    }

    // Вернуться в главное меню
    public void MainMenuButton()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

        Debug.Log("[PauseMenu] Возврат в главное меню");
    }
}