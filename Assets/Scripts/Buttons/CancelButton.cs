using UnityEngine;

using Language = LanguageSetting.Language;
using ScreenMode = ScreenModeSetting.ScreenMode;
using ScreenResolution = ScreenResolutionSetting.ScreenResolution;

public class CancelButton : BaseButton
{
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private LanguageSetting languageSetting;
    [SerializeField] private ScreenModeSetting screenModeSetting;
    [SerializeField] private ScreenResolutionSetting screenResolutionSetting;
    [SerializeField] private VolumeSettings volumeSettings;
        
    private int language;
    private int screenMode;
    private int screenResolution;
    private float masterVolume;
    private float bgmVolume;
    private float sfxVolume;

    void OnEnable()
    {
        language = PlayerPrefs.GetInt("Language");
        screenMode = PlayerPrefs.GetInt("ScreenMode");
        screenResolution = PlayerPrefs.GetInt("ScreenResolution");
        masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
    }
    
    public override void OnClick()
    {
        base.OnClick();
        languageSetting.SetLanguage(language);
        screenModeSetting.SetScreenMode(screenMode);
        screenResolutionSetting.SetScreenResolution(screenResolution);
        volumeSettings.SetMasterVolume(masterVolume);
        volumeSettings.SetBGMVolume(bgmVolume);
        volumeSettings.SetSFXVolume(sfxVolume);
        settingsPanel.gameObject.SetActive(false);
    }
}
