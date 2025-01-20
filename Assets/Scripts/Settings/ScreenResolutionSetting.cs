using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenResolutionSetting : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public enum ScreenResolution
    {
        VGA,        // 640x480
        DoubleVGA,  // 960x640
        XGA,        // 1024x768
        HD,         // 1280x720
        FullXGA,    // 1366x768
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
            case ScreenResolution.VGA:
                Screen.SetResolution(640, 480, Screen.fullScreenMode);
                break;
            case ScreenResolution.DoubleVGA:
                Screen.SetResolution(960, 640, Screen.fullScreenMode);
                break;
            case ScreenResolution.XGA:
                Screen.SetResolution(1024, 768, Screen.fullScreenMode);
                break;
            case ScreenResolution.HD:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case ScreenResolution.FullXGA:
                Screen.SetResolution(1366, 768, Screen.fullScreenMode);
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