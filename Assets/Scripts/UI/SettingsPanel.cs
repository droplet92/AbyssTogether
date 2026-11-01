using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private LanguageSetting languageSetting;
    [SerializeField] private ScreenModeSetting screenModeSetting;
    [SerializeField] private ScreenResolutionSetting screenResolutionSetting;
    [SerializeField] private VolumeSettings volumeSettings;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;
        
    private int language;
    private int screenMode;
    private int screenResolution;
    private float masterVolume;
    private float bgmVolume;
    private float sfxVolume;

    void Awake()
    {
        okButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Rollback);
    }
    void OnEnable()
    {
        language = GameSettings.Language;
        screenMode = GameSettings.ScreenMode;
        screenResolution = GameSettings.ScreenResolution;
        masterVolume = GameSettings.MasterVolume;
        bgmVolume = GameSettings.BgmVolume;
        sfxVolume = GameSettings.SfxVolume;
    }

    private void Confirm()
    {
        gameObject.SetActive(false);
    }
    private void Rollback()
    {
        languageSetting.SetLanguage(language);
        screenModeSetting.SetScreenMode(screenMode);
        screenResolutionSetting.SetScreenResolution(screenResolution);
        volumeSettings.SetMasterVolume(masterVolume);
        volumeSettings.SetBGMVolume(bgmVolume);
        volumeSettings.SetSFXVolume(sfxVolume);
        gameObject.SetActive(false);
    }
}
