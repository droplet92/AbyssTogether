using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private CanvasRenderer deckPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button deckButton;
    [SerializeField] private List<ItemUI> items;
    [SerializeField] private List<PotionUI> potions;
    
    public void Initialize()
    {
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        deckButton.onClick.AddListener(ToggleDeckPanel);
    }

    public void InitializeSlots()
    {
        foreach (var item in items)
            item.Initialize();

        foreach (var potion in potions)
            potion.Initialize();
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
