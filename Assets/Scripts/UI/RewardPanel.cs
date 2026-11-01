using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] private PotionDatabase potionDatabase;
    [SerializeField] private EquipmentDatabase equipmentDatabase;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text notice;
    [SerializeField] private LocalizedString potionDiscardedNotice;
    [SerializeField] private LocalizedString itemOwnedNotice;
    [SerializeField, Optional] private Button potionButton;
    [SerializeField, Optional] private Button itemButton;
    [SerializeField] private List<ItemUI> itemList;
    [SerializeField] private List<PotionUI> potionList;
    [SerializeField] private Button okButton;

    private LocalizedString localizedDescription;
    private LocalizedString localizedNotice;
    private Dictionary<string, int> toIndex = new Dictionary<string, int>()
    {
        { "MagicBook", 0 },
        { "Ring",      1 },
        { "Sword",     2 },
        { "Necklace",  3 },
    };

    public void Initialize()
    {
        Debug.Assert(potionList.Count == 3, "SerializeField size changed: Potion List");
    }
    void OnDestroy()
    {
        if (localizedDescription != null)
            localizedDescription.StringChanged -= OnStringChanged;

        HideNotice();
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

        HideNotice();
        for (int i = 0; i < SaveData.PotionSlotCount; i++)
        {
            if (SaveData.GetPotion(i + 1).Length == 0)
            {
                SaveData.SetPotion(i + 1, potion.name);
                potionList[i].SetPotionData();
                return;
            }
        }
        ShowNotice(potionDiscardedNotice);
    }
    public void ShowItem()
    {
        itemButton?.gameObject.SetActive(false);

        var item = equipmentDatabase.GetRandomEquipment();
        image.sprite = item.equipmentImage;

        localizedDescription = item.description;
        localizedDescription.StringChanged += OnStringChanged;
        localizedDescription.RefreshString();

        HideNotice();
        if (SaveData.HasItem(item.name))
            ShowNotice(itemOwnedNotice);

        SaveData.SetItem(item.name, true);
        itemList[toIndex[item.name]].SetItemData();
    }

    private void OnStringChanged(string value)
    {
        description.text = value;
    }

    private void ShowNotice(LocalizedString text)
    {
        localizedNotice = text;
        localizedNotice.StringChanged += OnNoticeChanged;
        localizedNotice.RefreshString();
        notice.gameObject.SetActive(true);
    }
    private void HideNotice()
    {
        if (localizedNotice != null)
            localizedNotice.StringChanged -= OnNoticeChanged;

        localizedNotice = null;
        notice.gameObject.SetActive(false);
    }
    private void OnNoticeChanged(string value)
    {
        notice.text = value;
    }
}
