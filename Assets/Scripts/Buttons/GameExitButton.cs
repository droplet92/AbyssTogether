using UnityEngine;

public class GameExitButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
