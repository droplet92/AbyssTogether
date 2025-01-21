using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    void Start()
    {
        if (PlayerPrefs.GetInt($"Item{itemName}") == 1)
        {
            var item = db.GetEquipment(itemName);
            isActive = true;
            tooltip = Instantiate(tooltipPrefab, statusBar).GetComponent<Tooltip>();

            tooltip.SetTooltipData(item.equipmentName, item.description);
            tooltip.gameObject.SetActive(false);
            image.gameObject.SetActive(true);

            var rectTransform = image.GetComponent<RectTransform>();
            tooltip.GetComponent<RectTransform>().position = rectTransform.position + new Vector3(230f, -25f, 0);
        }
        if (name == $"ItemSlot{PlayerPrefs.GetInt("PlayerCharacter")}")
            marker.gameObject.SetActive(true);
    }
    void Update()
    {
        bool isDied = false;
        
        if (character != null)
            isDied = !character.gameObject.activeSelf || character.isDied();

        else if (itemName == "MagicBook")
            isDied = PlayerPrefs.GetInt("HpHealer") == 0;

        else if (itemName == "Ring")
            isDied = PlayerPrefs.GetInt("HpMagician") == 0;

        else if (itemName == "Sword")
            isDied = PlayerPrefs.GetInt("HpSwordsman") == 0;

        else if (itemName == "Necklace")
            isDied = PlayerPrefs.GetInt("HpWarrior") == 0;

        if (isDied)
        {
            isActive = false;
            image.color = Color.gray;
            portrait.color = Color.gray;

            if (tooltip != null)
                tooltip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isActive)
            tooltip.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isActive)
            tooltip.gameObject.SetActive(false);
    }
}
