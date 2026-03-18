using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private const string MUSIC_KEY = "musicVolume";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (volumeSlider != null)
        {
            // Загружаем сохранённое значение
            volumeSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            AudioListener.volume = volumeSlider.value;
            UpdateVolumeText();
            volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        }
        else
        {
            // Нет слайдера (игровая сцена) — просто применяем сохранённое
            AudioListener.volume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        }
    }

    // Вызывается из Inspector через OnValueChanged
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        UpdateVolumeText();
        PlayerPrefs.SetFloat(MUSIC_KEY, volumeSlider.value);
        PlayerPrefs.Save();
    }

    // Вызывается из GameSettingsConnector в паузе
    public void ConnectMusicSlider(Slider slider, TextMeshProUGUI label = null)
    {
        if (slider == null) return;

        slider.onValueChanged.RemoveAllListeners();
        slider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);

        if (label != null)
            label.text = Mathf.RoundToInt(slider.value * 100f) + "%";

        slider.onValueChanged.AddListener((val) =>
        {
            AudioListener.volume = val;
            PlayerPrefs.SetFloat(MUSIC_KEY, val);
            PlayerPrefs.Save();
            if (label != null)
                label.text = Mathf.RoundToInt(val * 100f) + "%";
        });
    }

    private void UpdateVolumeText()
    {
        if (volumeText != null)
            volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100f) + "%";
    }
}