using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : AutoFieldValidator, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character character;
    [SerializeField] private Transform statusBar;
    [SerializeField] private EquipmentDatabase db;
    [SerializeField] private Image portrait;
    [SerializeField] private Image marker;
    [SerializeField] private Image image;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private string itemName;

    private Tooltip tooltip;
    private bool isActive = false;
    private Dictionary<string, string> toPreferenceName = new Dictionary<string, string>()
    {
        { "Necklace",    "HpWarrior" },
        { "Sword",       "HpSwordsman" },
        { "Ring",        "HpMagician" },
        { "MagicBook",   "HpHealer" },
    };

    void Start()
    {
        if (PlayerPrefs.GetInt($"Item{itemName}") == 1)
        {
            isActive = true;
            image.gameObject.SetActive(true);

            var item = db.GetEquipment(itemName);
            tooltip = Tooltip.FromPrefab(tooltipPrefab, statusBar, transform.localPosition + new Vector3(150f, 0f));
            tooltip.SetTooltipData(item.equipmentName, item.description);
        }
        bool isPlayerCharacter = name == $"ItemSlot{PlayerPrefs.GetInt("PlayerCharacter")}";
        if (isPlayerCharacter)
            marker.gameObject.SetActive(true);
    }
    void Update()
    {
        if (IsCharacterDead())
        {
            isActive = false;
            tooltip?.gameObject.SetActive(false);

            DisableColor();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive) return;
        tooltip.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive) return;
        tooltip.gameObject.SetActive(false);
    }

    private bool IsCharacterDead()
    {
        if (character == null) return PlayerPrefs.GetInt(toPreferenceName[itemName]) == 0;
        return character.IsDead();
    }
    private void DisableColor()
    {
        image.color = Color.gray;
        portrait.color = Color.gray;
    }
}
