using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance { get; private set; }

    [Header("FPS Settings")]
    [SerializeField] private TMP_Dropdown fpsDropdown;

    [Header("Display Mode Settings")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;

    [Header("Window Size (for Windowed mode)")]
    [SerializeField] private int windowedWidth = 1280;  // Размер окна
    [SerializeField] private int windowedHeight = 720;

    private int[] fpsOptions = { 30, 60, 120, -1 };
    private const string FPS_KEY = "TargetFPS";
    private const string DISPLAY_MODE_KEY = "DisplayMode";

    void Awake()
    {
        // Singleton - только один экземпляр на всю игру
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // НЕ УНИЧТОЖАТЬ при загрузке новой сцены
        DontDestroyOnLoad(gameObject);

        // Принудительно отключаем VSync
        QualitySettings.vSyncCount = 0;

        // Применяем сохранённые настройки сразу
        ApplySavedSettings();
    }

    void Start()
    {
        // Настраиваем FPS dropdown только если он есть (только в меню)
        if (fpsDropdown != null)
        {
            SetupFPSDropdown();
            LoadFPSDropdownValue();
        }

        // Настраиваем Display Mode dropdown
        if (displayModeDropdown != null)
        {
            SetupDisplayModeDropdown();
        }
    }

    // ===== FPS SETTINGS =====

    void SetupFPSDropdown()
    {
        fpsDropdown.ClearOptions();

        fpsDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "30 FPS",
            "60 FPS",
            "120 FPS",
            "Unlimited"
        });

        fpsDropdown.onValueChanged.AddListener(OnFPSChanged);
    }

    public void OnFPSChanged(int index)
    {
        int targetFPS = fpsOptions[index];
        SetFPS(targetFPS);
    }

    void SetFPS(int fps)
    {
        // Сначала отключаем VSync!
        QualitySettings.vSyncCount = 0;

        // Потом устанавливаем FPS
        Application.targetFrameRate = fps;

        // Сохраняем
        PlayerPrefs.SetInt(FPS_KEY, fps);
        PlayerPrefs.Save();

        Debug.Log($"[SettingsManager] Target FPS: {fps}");
    }

    void LoadFPSDropdownValue()
    {
        int savedFPS = PlayerPrefs.GetInt(FPS_KEY, 60);
        int dropdownIndex = System.Array.IndexOf(fpsOptions, savedFPS);

        if (dropdownIndex >= 0 && fpsDropdown != null)
        {
            // Отключаем событие, чтобы не вызвать повторное сохранение
            fpsDropdown.onValueChanged.RemoveListener(OnFPSChanged);
            fpsDropdown.value = dropdownIndex;
            fpsDropdown.onValueChanged.AddListener(OnFPSChanged);
        }
    }

    // ===== DISPLAY MODE SETTINGS =====

    void SetupDisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();

        // Добавляем варианты
        displayModeDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Fullscreen",        // 0 - Полноэкранный (эксклюзивный)
            "Windowed",          // 1 - Оконный (меньшего размера)
            "Borderless"         // 2 - Безрамочное окно (на весь экран)
        });

        // Загружаем сохранённое значение
        int savedMode = PlayerPrefs.GetInt(DISPLAY_MODE_KEY, 0);

        // Устанавливаем значение без вызова события
        displayModeDropdown.onValueChanged.RemoveAllListeners();
        displayModeDropdown.value = savedMode;

        // Подписываемся на изменение
        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeChanged);
    }

    public void OnDisplayModeChanged(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0: // Fullscreen - настоящий полный экран
                Screen.SetResolution(
                    Screen.currentResolution.width,
                    Screen.currentResolution.height,
                    FullScreenMode.ExclusiveFullScreen
                );
                Debug.Log("[SettingsManager] Режим: Fullscreen (Exclusive)");
                break;

            case 1: // Windowed - окно меньшего размера
                Screen.SetResolution(
                    windowedWidth,
                    windowedHeight,
                    FullScreenMode.Windowed
                );
                Debug.Log($"[SettingsManager] Режим: Windowed ({windowedWidth}x{windowedHeight})");
                break;

            case 2: // Borderless - окно на весь экран без рамки
                Screen.SetResolution(
                    Screen.currentResolution.width,
                    Screen.currentResolution.height,
                    FullScreenMode.FullScreenWindow
                );
                Debug.Log("[SettingsManager] Режим: Borderless Window");
                break;
        }

        // Сохраняем
        PlayerPrefs.SetInt(DISPLAY_MODE_KEY, modeIndex);
        PlayerPrefs.Save();
    }

    // ===== ПРИМЕНЕНИЕ НАСТРОЕК ПРИ ЗАПУСКЕ =====

    void ApplySavedSettings()
    {
        // Применяем FPS
        int savedFPS = PlayerPrefs.GetInt(FPS_KEY, 60);
        SetFPS(savedFPS);

        // Применяем Display Mode
        int savedDisplayMode = PlayerPrefs.GetInt(DISPLAY_MODE_KEY, 0);
        OnDisplayModeChanged(savedDisplayMode);
    }

    // ===== ПОДКЛЮЧЕНИЕ DROPDOWNS ИЗ GAME СЦЕНЫ =====

    public void ConnectGameDropdowns(TMP_Dropdown gameFpsDropdown, TMP_Dropdown gameDisplayDropdown)
    {
        // Подключаем новые Dropdowns
        if (gameFpsDropdown != null)
        {
            fpsDropdown = gameFpsDropdown;
            SetupFPSDropdown();
            LoadFPSDropdownValue();
        }

        if (gameDisplayDropdown != null)
        {
            displayModeDropdown = gameDisplayDropdown;
            SetupDisplayModeDropdown();
        }

        Debug.Log("[SettingsManager] Game Dropdowns подключены");
    }

} // ← Последняя закрывающая скобка класса