using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionUI : AutoFieldValidator, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform statusBar;
    [SerializeField] private PotionDatabase db;
    [SerializeField] private Image image;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private int slotCount;
    [SerializeField] private bool isDraggable;

    private Tooltip tooltip;
    private Canvas canvas;
    private FieldManager fieldPanel;
    private RectTransform rectTransform;
    private Vector3 originPosition;
    private Quaternion originLocalRotation;
    private ITarget hoveredTarget;
    private bool isActive = false;
    private string potionName;
    private string targetType;

    void Start()
    {
        tooltip = Tooltip.FromPrefab(tooltipPrefab, statusBar, transform.parent.localPosition);
        canvas = GetComponentInParent<Canvas>();
        fieldPanel = canvas.GetComponentInChildren<FieldManager>();
        rectTransform = GetComponent<RectTransform>();
            
        originPosition = transform.position;
        originLocalRotation = transform.localRotation;

        SetPotionData();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive || eventData.dragging) return;
        tooltip.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive || eventData.dragging) return;
        tooltip.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable) return;
        tooltip.gameObject.SetActive(false);
        
        DOTween.Sequence()
            .Join(transform.DOScale(0.5f, 0.2f))
            .Join(transform.DOMove(eventData.position, 0))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 30f), 0.2f));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable) return;
            
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        var target = ITargetUtil.GetTargetUnderMouse(canvas);
        if (target != hoveredTarget)
        {
            if (hoveredTarget != null)
                fieldPanel.SetHighlight(targetType, hoveredTarget, false);

            hoveredTarget = target;

            if (hoveredTarget?.CanApply(targetType) ?? false)
                fieldPanel.SetHighlight(targetType, hoveredTarget, true);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable) return;
            
        if (hoveredTarget?.CanApply(targetType) ?? false)
        {
            isActive = false;
            image.color = new Color32(0, 0, 0, 0);

            PlayerPrefs.SetString($"Potion{slotCount}", null);
            tooltip.gameObject.SetActive(false);
            fieldPanel.SetHighlight(targetType, hoveredTarget, false);
            fieldPanel.ApplyEffect(targetType, null, hoveredTarget, potionName);
            hoveredTarget = null;
        }
        else
        {
            DOTween.Sequence()
                .Join(transform.DOScale(1f, 0.1f))
                .Join(transform.DOMove(originPosition, 0.1f))
                .Join(transform.DOLocalRotateQuaternion(originLocalRotation, 0.1f))
                .SetEase(Ease.OutQuad)
                .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(statusBar));
        }
    }

    public void SetPotionData()
    {
        string name = PlayerPrefs.GetString($"Potion{slotCount}");
        if (name.Length == 0) return;
        
        isActive = true;
        image.color = Color.white;

        var potion = db.GetPotion(name);
        potionName = potion.name;
        targetType = potion.targetType;
        image.sprite = potion.potionImage;
        tooltip.SetTooltipData(potion.potionName, potion.description);
    }
}