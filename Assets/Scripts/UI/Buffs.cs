using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buffs : AutoFieldValidator, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform field;
    [SerializeField] private Image attackImage;
    [SerializeField] private Image defenseImage;
    [SerializeField] private GameObject tooltipPrefab;

    private BuffTooltip tooltip;
    private bool isActive = false;
    private int attack = 0;
    private int defense = 0;
    
    void Awake()
    {
        bool isCharacter = transform.parent.GetComponent<Character>() != null;
        var localPosition = transform.parent.localPosition + (isCharacter ? new Vector3(0, 200f) : Vector3.zero);
        tooltip = BuffTooltip.FromPrefab(tooltipPrefab, field, localPosition);
        UpdateContents(0, 0);
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
    
    public void UpdateContents(int da, int dd)
    {
        attack += da;
        defense += dd;

        bool hasValue = attack != 0 || defense != 0;
        if (hasValue)
        {
            isActive = true;

            tooltip.SetTooltipData(attack, defense);
            UpdateIconColor();
        }
        else
        {
            isActive = false;
        }
        tooltip?.gameObject.SetActive(false);   // monster may not be summoned
        attackImage.gameObject.SetActive(attack != 0);
        defenseImage.gameObject.SetActive(defense != 0);
    }

    private void UpdateIconColor()
    {
        attackImage.color = (attack > 0) ? Color.red : Color.blue;
        defenseImage.color = (defense > 0) ? Color.red : Color.blue;
    }
}
