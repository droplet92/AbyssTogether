using UnityEngine;
using UnityEngine.UI;

public class StatusBar : AutoFieldValidator
{
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private CanvasRenderer deckPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button deckButton;
    
    void Awake()
    {
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        deckButton.onClick.AddListener(ToggleDeckPanel);
    }

    private void ToggleSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
    }
    private void ToggleDeckPanel()
    {
        deckPanel.gameObject.SetActive(!deckPanel.gameObject.activeSelf);
    }
}
