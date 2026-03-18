using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettingsConnector : MonoBehaviour
{
    [Header("FPS и Display дропдауны")]
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown displayModeDropdown;

    [Header("Слайдер музыки")]
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeLabel;

    void Start()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.ConnectGameDropdowns(fpsDropdown, displayModeDropdown);
        else
            Debug.LogError("[GameSettingsConnector] SettingsManager не найден!");

        if (SoundManager.Instance != null)
            SoundManager.Instance.ConnectMusicSlider(musicVolumeSlider, musicVolumeLabel);
        else
            Debug.LogError("[GameSettingsConnector] SoundManager не найден!");
    }
}