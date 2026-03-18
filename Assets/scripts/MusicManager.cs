using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance;

    void Awake()
    {
        // Проверяем, есть ли уже экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке сцен
            LoadVolume(); // Загружаем громкость при старте
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликат
        }
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume", 1f);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
}