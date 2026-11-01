using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private static string MASTER_VOL = "MasterVolume";
    private static string BGM_VOL = "BGMVolume";
    private static string SFX_VOL = "SFXVolume";

    void Awake()
    {
        float defaultMaster = GameSettings.MasterVolume;
        float defaultBGM    = GameSettings.BgmVolume;
        float defaultSFX    = GameSettings.SfxVolume;

        masterSlider.value = defaultMaster;
        bgmSlider.value    = defaultBGM;
        sfxSlider.value    = defaultSFX;

        SetMasterVolume(defaultMaster);
        SetBGMVolume(defaultBGM);
        SetSFXVolume(defaultSFX);
    }

    public void SetMasterVolume(float value)
    {
        float clampedValue = Mathf.Max(value, 0.0001f);
        float dB = Mathf.Log10(clampedValue) * 20f;

        masterSlider.SetValueWithoutNotify(value);
        audioMixer.SetFloat(MASTER_VOL, dB);
        GameSettings.MasterVolume = value;
    }

    public void SetBGMVolume(float value)
    {
        float clampedValue = Mathf.Max(value, 0.0001f);
        float dB = Mathf.Log10(clampedValue) * 20f;

        bgmSlider.SetValueWithoutNotify(value);
        audioMixer.SetFloat(BGM_VOL, dB);
        GameSettings.BgmVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        float clampedValue = Mathf.Max(value, 0.0001f);
        float dB = Mathf.Log10(clampedValue) * 20f;
        
        sfxSlider.SetValueWithoutNotify(value);
        audioMixer.SetFloat(SFX_VOL, dB);
        GameSettings.SfxVolume = value;
    }
}
