using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenResolutionSetting : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public enum ScreenResolution
    {
        nHD,        // 640x360
        SD,         // 854x480
        qHD,        // 960x540
        HD,         // 1280x720
        FWXGA,      // 1366x768
        HDPlus,     // 1600x900
        FullHD,     // 1920x1080
        WideQHD,    // 2560x1440
        UltraHD     // 3840x2160
    }

    private void Start()
    {
        int value = PlayerPrefs.GetInt("ScreenMode", (int)ScreenResolution.FullHD);
        
        SetScreenResolution(value);
    }
    public void SetScreenResolution(int value)
    {
        var selection = (ScreenResolution)value;
        dropdown.value = value;

        switch (selection)
        {
            case ScreenResolution.nHD:
                Screen.SetResolution(640, 360, Screen.fullScreenMode);
                break;
            case ScreenResolution.SD:
                Screen.SetResolution(854, 480, Screen.fullScreenMode);
                break;
            case ScreenResolution.qHD:
                Screen.SetResolution(960, 540, Screen.fullScreenMode);
                break;
            case ScreenResolution.HD:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case ScreenResolution.FWXGA:
                Screen.SetResolution(1366, 768, Screen.fullScreenMode);
                break;
            case ScreenResolution.HDPlus:
                Screen.SetResolution(1600, 900, Screen.fullScreenMode);
                break;
            case ScreenResolution.FullHD:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case ScreenResolution.WideQHD:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
            case ScreenResolution.UltraHD:
                Screen.SetResolution(3840, 2160, Screen.fullScreenMode);
                break;
            default:
                Debug.LogError("미구현 모드입니다.");
                break;
        }
    }
}