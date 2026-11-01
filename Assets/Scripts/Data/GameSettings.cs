using UnityEngine;

public static class GameSettings
{
    public static int Language
    {
        get => PlayerPrefs.GetInt("Language", (int)LanguageSetting.Language.English);
        set => PlayerPrefs.SetInt("Language", value);
    }
    public static int ScreenMode
    {
        get => PlayerPrefs.GetInt("ScreenMode", (int)ScreenModeSetting.ScreenMode.FullScreenWindow);
        set => PlayerPrefs.SetInt("ScreenMode", value);
    }
    public static int ScreenResolution
    {
        get => PlayerPrefs.GetInt("ScreenResolution", (int)ScreenResolutionSetting.ScreenResolution.FullHD);
        set => PlayerPrefs.SetInt("ScreenResolution", value);
    }
    public static float MasterVolume
    {
        get => PlayerPrefs.GetFloat("MasterVolume", 1f);
        set => PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public static float BgmVolume
    {
        get => PlayerPrefs.GetFloat("BGMVolume", 1f);
        set => PlayerPrefs.SetFloat("BGMVolume", value);
    }
    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat("SFXVolume", 1f);
        set => PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
