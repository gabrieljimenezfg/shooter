using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum FPSLimit
{
    Limit30,
    Limit60,
    Limit120,
    Unlimited
}

public class GameSettings
{
    public float musicVolume;
    public float sfxVolume;
    public bool fullscreen;
    public FPSLimit fpsLimit;
    public int quality;
    public int resolution;
}

public class OptionsManager : MonoBehaviour
{
    private const string PLAYE_PREFS_SETTINGS = "PLAYE_PREFS_SETTINGS";

    [Header("UI")] [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown fpsLimitDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Volume")] [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Buttons")] [SerializeField] private Button applyButton;
    [SerializeField] private Button backButton;

    private GameSettings gameSettings;

    private void Awake()
    {
        gameSettings = new GameSettings();

        applyButton.onClick.AddListener(ApplySettings);
        backButton.onClick.AddListener(GoBack);
    }

    private void Start()
    {
        LoadSettings();
        SetUIElements();
    }

    private void SetAvailableResolutions()
    {
        resolutionDropdown.ClearOptions();
        var resolutionOptions = Screen.resolutions;
        foreach (var resolution in resolutionOptions)
        {
            var option = resolution.width + "x" + resolution.height;
            var tmpDropdownOption = new TMP_Dropdown.OptionData(option);
            resolutionDropdown.options.Add(tmpDropdownOption);
        }

        resolutionDropdown.value = gameSettings.resolution;
    }

    private void SetAvailableQualityOptions()
    {
        qualityDropdown.ClearOptions();
        var qualityOptions = QualitySettings.names;
        var tmpDropdownOptionsList = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < qualityOptions.Length; i++)
        {
            var optionToTMP = new TMP_Dropdown.OptionData(qualityOptions[i]);
            tmpDropdownOptionsList.Add(optionToTMP);
        }

        qualityDropdown.AddOptions(tmpDropdownOptionsList);
        qualityDropdown.value = gameSettings.quality;
    }

    private void SaveSettings()
    {
        SavesUtility.SaveGame(gameSettings, PLAYE_PREFS_SETTINGS);
    }

    private void LoadSettings()
    {
        var loadedSave = SavesUtility.GetLoadedSave<GameSettings>(PLAYE_PREFS_SETTINGS);
        if (loadedSave != null)
        {
            gameSettings = loadedSave;
        }
        else
        {
            SetDefaultDataValues();
        }
    }

    private void SetDefaultDataValues()
    {
        gameSettings.musicVolume = 1;
        gameSettings.sfxVolume = 1;
        gameSettings.fullscreen = false;
        gameSettings.quality = 1;
        gameSettings.fpsLimit = FPSLimit.Limit120;

        Resolution[] resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == gameSettings.resolution && resolutions[i].height == gameSettings.resolution)
            {
                gameSettings.resolution = i;
                break;
            }
        }
    }

    private void ApplySettings()
    {
        ApplySound();
        ApplyGraphics();

        SaveSettings();
    }

    private void ApplySound()
    {
        gameSettings.musicVolume = musicVolumeSlider.value;
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        AudioManager.Instance.SetMusicVolume(gameSettings.musicVolume);
        AudioManager.Instance.SetSFXVolume(gameSettings.sfxVolume);
    }

    private void ApplyGraphics()
    {
        SaveFPSLimit();
        SaveResolution();

        gameSettings.fullscreen = fullscreenToggle.isOn;
        Screen.fullScreen = gameSettings.fullscreen;

        gameSettings.quality = qualityDropdown.value;
        QualitySettings.SetQualityLevel(gameSettings.quality);
    }

    private void SaveResolution()
    {
        gameSettings.resolution = resolutionDropdown.value;
        var newResolution = Screen.resolutions[gameSettings.resolution];
        Screen.SetResolution(newResolution.width, newResolution.height, gameSettings.fullscreen);
    }

    private void SaveFPSLimit()
    {
        gameSettings.fpsLimit = (FPSLimit)fpsLimitDropdown.value;
        var targetFPSLimit = 0;

        switch (gameSettings.fpsLimit)
        {
            case FPSLimit.Limit30:
                targetFPSLimit = 30;
                break;
            case FPSLimit.Limit60:
                targetFPSLimit = 60;
                break;
            case FPSLimit.Limit120:
                targetFPSLimit = 120;
                break;
            case FPSLimit.Unlimited:
                targetFPSLimit = -1;
                break;
        }

        Application.targetFrameRate = targetFPSLimit;
    }

    private void SetUIElements()
    {
        musicVolumeSlider.value = gameSettings.musicVolume;
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        fpsLimitDropdown.value = (int)gameSettings.fpsLimit;
        SetAvailableResolutions();
        SetAvailableQualityOptions();
    }

    private void GoBack()
    {
    }
}