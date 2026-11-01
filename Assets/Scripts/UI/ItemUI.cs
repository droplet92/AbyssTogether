using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform statusBar;
    [SerializeField] private EquipmentDatabase db;
    [SerializeField] private Image portrait;
    [SerializeField] private Image marker;
    [SerializeField] private Image image;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private string itemName;

    public string ItemName => itemName;

    private Character character;
    private Tooltip tooltip;
    private bool isActive = false;

    public void Initialize(IReadOnlyList<Character> characters)
    {
        var owner = db.GetEquipment(itemName).owner.ToString();
        character = characters?.FirstOrDefault(c => c.name == owner);

        SetItemData();
        
        bool isPlayerCharacter = name == $"ItemSlot{SaveData.PlayerCharacter}";
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
        if (character == null) return SaveData.GetHealth(db.GetEquipment(itemName).owner.ToString()) <= 0;
        return character.IsDead();
    }
    private void DisableColor()
    {
        image.color = Color.gray;
        portrait.color = Color.gray;
    }
    public void SetItemData()
    {
        if (!SaveData.HasItem(itemName)) return;

        isActive = true;
        image.gameObject.SetActive(true);

        var item = db.GetEquipment(itemName);
        tooltip = Tooltip.FromPrefab(tooltipPrefab, statusBar, transform.localPosition + new Vector3(150f, 0f));
        tooltip.SetTooltipData(item.equipmentName, item.description);
    }
}
