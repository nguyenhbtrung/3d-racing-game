using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject panelPickVehicle;
    [SerializeField] private GameObject panelSelectMap;
    [SerializeField] private Transform mainMenu;

    [Header("Settings menu")]
    [SerializeField] private OptionButtonAnimation[] optionButtonAnimations;
    [SerializeField] private Transform[] optionContents;
    [SerializeField] private Transform settingPanel;
    private int selectedIndex = 1;

    [Header("Settings Options")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private TMPro.TextMeshProUGUI OverallVolumePercentageText;
    [SerializeField] private Image overallVolumeImage;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite unmuteSprite;

    [SerializeField] private ToggleGroup qualityToggleGroup;

    [SerializeField] private ToggleGroup resolutionToggleGroup;
    [SerializeField] private GameObject optionTogglePrefab;

    [SerializeField] private Toggle modeHaveNormalBotToggle;


    private Resolution[] resolutions;
    private bool isMute = false;
    private float currentOverallVolume;

    private void Start()
    {
        GetResolution();
    }

    public void HandleOptionButtonClick(int index)
    {
        if (selectedIndex == index)
        {
            return;
        }
        optionContents[selectedIndex].gameObject.SetActive(false);
        optionButtonAnimations[selectedIndex].Deselect();

        selectedIndex = index;

        optionContents[selectedIndex].gameObject.SetActive(true);
        optionButtonAnimations[selectedIndex].Select();

    }

    public void OpenSetting()
    {
        mainMenu.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(true);
        GetSettingValues();
        HandleOptionButtonClick(0);
        
    }

    private void GetSettingValues()
    {
        GetQuality();
        GetVolume();
        GetMode();
    }

    private void GetQuality()
    {
        int qualityIndex = QualitySettings.GetQualityLevel();
        Transform option = qualityToggleGroup.transform.GetChild(qualityIndex);
        option.GetComponentInChildren<Toggle>().isOn = true;
    }

    private void GetResolution()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string optionString = resolutions[i].width + " x " + resolutions[i].height;
            var optionToggle = Instantiate(optionTogglePrefab, resolutionToggleGroup.transform);
            Toggle toggle = optionToggle.GetComponentInChildren<Toggle>();
            toggle.group = resolutionToggleGroup;
            toggle.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(optionString);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                toggle.isOn = true;
            }

            int index = i;
            toggle.onValueChanged.AddListener(delegate
            {
                SetResolution(index);
            });
        }
    }

    private void GetVolume()
    {
        audioMixer.GetFloat("Volume", out float volume);
        overallVolumeSlider.value = volume;
        float max = overallVolumeSlider.maxValue;
        float min = overallVolumeSlider.minValue;
        int percentage = (int)((volume - min) / (max - min) * 100);
        OverallVolumePercentageText.SetText(percentage.ToString() + "%");

    }

    public void GetMode()
    {
        modeHaveNormalBotToggle.isOn = GameData.Instance.HaveNormalBot;
    }
    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, overallVolumeSlider.minValue, overallVolumeSlider.maxValue);
        audioMixer.SetFloat("Volume", volume);
        GetVolume();
    }

    public void Mute()
    {
        currentOverallVolume = overallVolumeSlider.value;
        SetVolume(overallVolumeSlider.minValue);
        overallVolumeImage.sprite = muteSprite;
    }
    public void Unmute()
    {
        SetVolume(currentOverallVolume);
        overallVolumeImage.sprite = unmuteSprite;
    }

    public void HandleVolumeButtonClick()
    {
        if (!isMute)
        {
            Mute();
            isMute = true;
        }
        else
        {
            Unmute();
            isMute = false;
        }
    }

    public void HandleIncreaseButtonClick()
    {
        float max = overallVolumeSlider.maxValue;
        float min = overallVolumeSlider.minValue;
        float newVolume = overallVolumeSlider.value + (max - min) / 100;
        SetVolume(newVolume);
    }

    public void HandleDecreaseButtonClick()
    {
        float max = overallVolumeSlider.maxValue;
        float min = overallVolumeSlider.minValue;
        float newVolume = overallVolumeSlider.value - (max - min) / 100;
        SetVolume(newVolume);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetMode(bool haveNormalBot)
    {
        GameData.Instance.HaveNormalBot = haveNormalBot;
    }

    public void CloseSettings()
    {
        settingPanel.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }


    public void OpenPickVehicle()
    {
        panelPickVehicle.SetActive(true);
    }

    public void ClosePickVehicle()
    {
        panelPickVehicle.SetActive(false);
    }

    public void OpenSelectMap()
    {
        panelSelectMap.SetActive(true);
    }

    public void CloseSelectMap()
    {
        panelSelectMap.SetActive(false);
    }

    public void QuitApp()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
