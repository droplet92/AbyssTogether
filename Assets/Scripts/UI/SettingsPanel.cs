using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : AutoFieldValidator
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

    protected override void Awake()
    {
        base.Awake();
        okButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Rollback);
    }
    void OnEnable()
    {
        language = PlayerPrefs.GetInt("Language", (int)LanguageSetting.Language.English);
        screenMode = PlayerPrefs.GetInt("ScreenMode", (int)ScreenModeSetting.ScreenMode.FullScreenWindow);
        screenResolution = PlayerPrefs.GetInt("ScreenResolution", (int)ScreenResolutionSetting.ScreenResolution.FullHD);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
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
