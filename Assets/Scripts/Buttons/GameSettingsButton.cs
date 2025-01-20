using UnityEngine;

public class GameSettingsButton : BaseButton
{
    [SerializeField] private CanvasRenderer settingsPanel;

    public override void OnClick()
    {
        base.OnClick();
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
    }
}
