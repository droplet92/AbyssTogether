using UnityEngine;

public class GameSettingsButton : MonoBehaviour
{
    [SerializeField] private CanvasRenderer settingsPanel;

    public void OnClick()
    {
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
    }
}
