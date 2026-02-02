using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class RewardPanel : AutoFieldValidator
{
    [SerializeField] private PotionDatabase potionDatabase;
    [SerializeField] private EquipmentDatabase equipmentDatabase;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Button potionButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private List<ItemUI> itemList;
    [SerializeField] private List<PotionUI> potionList;
    [SerializeField] private Button okButton;

    private LocalizedString localizedDescription;
    private Dictionary<string, int> toIndex = new Dictionary<string, int>()
    {
        { "MagicBook", 0 },
        { "Ring",      1 },
        { "Sword",     2 },
        { "Necklace",  3 },
    };

    void Awake()
    {
        Debug.Assert(potionList.Count == 3, "SerializeField size changed: Potion List");
    }
    void OnDestroy()
    {
        localizedDescription.StringChanged -= OnStringChanged;
    }

    public void AddConfirmListener(Action action)
    {
        okButton.onClick.AddListener(() => action());
    }
    public void ShowPotion()
    {
        potionButton.gameObject.SetActive(false);

        var potion = potionDatabase.GetRandomPotion();
        image.sprite = potion.potionImage;

        localizedDescription = potion.description;
        localizedDescription.StringChanged += OnStringChanged;
        localizedDescription.RefreshString();

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString($"Potion{i + 1}").Length == 0)
            {
                PlayerPrefs.SetString($"Potion{i + 1}", potion.name);
                potionList[i].SetPotionData();
                return;
            }
        }
    }
    public void ShowItem()
    {
        itemButton?.gameObject.SetActive(false);

        var item = equipmentDatabase.GetRandomEquipment();
        image.sprite = item.equipmentImage;

        localizedDescription = item.description;
        localizedDescription.StringChanged += OnStringChanged;
        localizedDescription.RefreshString();
        
        PlayerPrefs.SetInt($"Item{item.name}", 1);
        itemList[toIndex[item.name]].SetItemData();
    }

    private void OnStringChanged(string value)
    {
        description.text = value;
    }
}
