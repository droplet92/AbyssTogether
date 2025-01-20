using UnityEngine;

public class OkButton : BaseButton
{
    [SerializeField] private CanvasRenderer settingsPanel;

    public override void OnClick()
    {
        base.OnClick();
        settingsPanel.gameObject.SetActive(false);
    }
}
