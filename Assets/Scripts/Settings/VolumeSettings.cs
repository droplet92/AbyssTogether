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

    private readonly string MASTER_VOL = "MasterVolume";
    private readonly string BGM_VOL = "BGMVolume";
    private readonly string SFX_VOL = "SFXVolume";

    void Awake()
    {
        float defaultMaster = PlayerPrefs.GetFloat(MASTER_VOL, 1f);
        float defaultBGM    = PlayerPrefs.GetFloat(BGM_VOL,    1f);
        float defaultSFX    = PlayerPrefs.GetFloat(SFX_VOL,    1f);

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
        PlayerPrefs.SetFloat(MASTER_VOL, value);
    }

    public void SetBGMVolume(float value)
    {
        float clampedValue = Mathf.Max(value, 0.0001f);
        float dB = Mathf.Log10(clampedValue) * 20f;

        bgmSlider.SetValueWithoutNotify(value);
        audioMixer.SetFloat(BGM_VOL, dB);
        PlayerPrefs.SetFloat(BGM_VOL, value);
    }

    public void SetSFXVolume(float value)
    {
        float clampedValue = Mathf.Max(value, 0.0001f);
        float dB = Mathf.Log10(clampedValue) * 20f;
        
        sfxSlider.SetValueWithoutNotify(value);
        audioMixer.SetFloat(SFX_VOL, dB);
        PlayerPrefs.SetFloat(SFX_VOL, value);
    }
}
