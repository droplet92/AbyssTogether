using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button deckButton;
    [SerializeField] private List<ItemUI> items;
    [SerializeField] private List<PotionUI> potions;

    private SettingsPanel settingsPanel;
    private DeckPanel deckPanel;
    
    public void Initialize(SettingsPanel settingsPanel, DeckPanel deckPanel)
    {
        this.settingsPanel = settingsPanel;
        this.deckPanel = deckPanel;

        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        deckButton.onClick.AddListener(ToggleDeckPanel);
    }

    public void InitializeSlots(IReadOnlyList<Character> characters)
    {
        foreach (var item in items)
            item.Initialize(characters);

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
