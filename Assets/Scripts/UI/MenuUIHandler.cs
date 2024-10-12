using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
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


    private bool isMute = false;
    private float currentOverallVolume;

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
        GetVolume();
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
