#if UNITY_EDITOR
using UnityEditor;
#elif !UNITY_WEBGL
using UnityEngine;
#endif

public class GameExitButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #elif UNITY_WEBGL
    #else
        Application.Quit();
    #endif
    }
}
