using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buffs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image attackImage;
    [SerializeField] private Image defenseImage;
    [SerializeField] private GameObject tooltipPrefab;

    private BuffTooltip tooltip;
    private bool isActive = false;
    private int attack = 0;
    private int defense = 0;
    
    void Awake()
    {
        tooltip = Instantiate(tooltipPrefab, transform).GetComponent<BuffTooltip>();
        tooltip.gameObject.SetActive(false);
        UpdateContents(0, 0);
    }
    public void UpdateContents(int da, int dd)
    {
        attack += da;
        defense += dd;

        if (attack != 0 || defense != 0)
        {
            isActive = true;

            Debug.Log(this);
            Debug.Log(tooltip);
            Debug.Log(tooltip.gameObject);
            tooltip.gameObject.SetActive(true);
            tooltip.SetTooltipData(attack, defense);
            tooltip.gameObject.SetActive(false);

            attackImage.color = (attack > 0) ? Color.red : Color.blue;
            defenseImage.color = (defense > 0) ? Color.red : Color.blue;

            var rectTransform = GetComponent<RectTransform>();
            tooltip.GetComponent<RectTransform>().position = rectTransform.position + new Vector3(130f, -25f, 0);
        }
        else
        {
            if (isActive)
                tooltip.gameObject.SetActive(false);

            isActive = false;
        }
        attackImage.gameObject.SetActive(attack != 0);
        defenseImage.gameObject.SetActive(defense != 0);
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
