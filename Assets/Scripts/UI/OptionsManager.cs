using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Quality
{
    Low,
    Medium,
    High,
    Epic
}

public class GameSettings
{
    public float musicVolume;
    public float sfxVolume;
    public bool fullscreen;
    public int fpsLimit;
    public Quality quality;
    public int resolution;
}

public class OptionsManager : MonoBehaviour
{
    [Header("UI")] [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown fpsLimitDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Volume")] [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    private GameSettings gameSettings;

    private void Start()
    {
        LoadSettings();
        SetUIElements();
    }

    private void LoadSettings()
    {
    }

    private void SaveSettings()
    {
    }

    private void SetUIElements()
    {
        musicVolumeSlider.value = gameSettings.musicVolume;
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        fpsLimitDropdown.value = gameSettings.fpsLimit;
        qualityDropdown.value = (int)gameSettings.quality;
    }
}