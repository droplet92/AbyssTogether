using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ScreenModeSetting : MonoBehaviour
{
    [SerializeField] private List<LocalizedString> optionStrings; 
    [SerializeField] TMP_Dropdown dropdown;

    public enum ScreenMode
    {
        FullScreenWindow,
        Window
    }

    void Awake()
    {
    #if UNITY_WEBGL
        enabled = false;
        dropdown.interactable = false;
    #else
        int value = PlayerPrefs.GetInt("ScreenMode", (int)ScreenMode.FullScreenWindow);
        SetScreenMode(value);
    #endif
    }
    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateDropdownOptions();
    }
    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }
    private void OnLocaleChanged(Locale locale)
    {
        UpdateDropdownOptions();
    }
    public void SetScreenMode(int value)
    {
        var selection = (ScreenMode)value;
        dropdown.value = value;

        switch (selection)
        {
            case ScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Debug.LogError("미구현 모드입니다.");
                break;
        }
    }
    private void UpdateDropdownOptions()
    {
        dropdown.options.Clear();

        for (int i = 0; i < optionStrings.Count; i++)
        {
            string localizedText = optionStrings[i].GetLocalizedString();
            dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText));
        }
        dropdown.RefreshShownValue();
    }
}