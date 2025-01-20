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
    [SerializeField] private Button potionButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private List<ItemUI> itemList;
    [SerializeField] private List<PotionUI> potionList;

    private LocalizedString localizedDescription;

    void Start()
    {
        localizedDescription.StringChanged += OnStringChanged;

        localizedDescription.RefreshString();
    }
    void OnDestroy()
    {
        localizedDescription.StringChanged -= OnStringChanged;
    }

    public void ShowPotion()
    {
        var potion = potionDatabase.GetRandomPotion();
        image.sprite = potion.potionImage;
        localizedDescription = potion.description;
        localizedDescription.StringChanged += OnStringChanged;

        if (PlayerPrefs.GetString("Potion1").Length == 0)
        {
            PlayerPrefs.SetString("Potion1", potion.name);
            potionList[0].SetPotionData();
        }
        else if (PlayerPrefs.GetString("Potion2").Length == 0)
        {
            PlayerPrefs.SetString("Potion2", potion.name);
            potionList[1].SetPotionData();
        }
        else if (PlayerPrefs.GetString("Potion3").Length == 0)
        {
            PlayerPrefs.SetString("Potion3", potion.name);
            potionList[2].SetPotionData();
        }
        potionButton.gameObject.SetActive(false);
        localizedDescription.RefreshString();
    }
    public void ShowItem()
    {
        var item = equipmentDatabase.GetRandomEquipment();
        image.sprite = item.equipmentImage;
        localizedDescription = item.description;
        localizedDescription.StringChanged += OnStringChanged;

        PlayerPrefs.SetInt($"Item{item.name}", 1);
        localizedDescription.RefreshString();

        if (itemButton != null)
            itemButton.gameObject.SetActive(false);

        if (item.name == "MagicBook")
            itemList[0].gameObject.SetActive(true);

        else if (item.name == "Ring")
            itemList[1].gameObject.SetActive(true);

        else if (item.name == "Sword")
            itemList[2].gameObject.SetActive(true);

        else if (item.name == "Necklace")
            itemList[3].gameObject.SetActive(true);
    }

    private void OnStringChanged(string value)
    {
        description.text = value;
    }
}
