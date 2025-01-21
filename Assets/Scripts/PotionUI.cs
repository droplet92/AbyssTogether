using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform statusBar;
    [SerializeField] private PotionDatabase db;
    [SerializeField] private Image image;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private int slotCount;
    [SerializeField] private bool isDraggable;

    private Tooltip tooltip;
    private string potionName;
    private bool isActive = false;
    private string targetType;
    private Canvas canvas;
    private FieldManager fieldPanel;
    private RectTransform rectTransform;
    private Vector3 originPosition;
    private Quaternion originLocalRotation;
    private ICardTarget hoveredTarget;

    void Start()
    {
        tooltip = Instantiate(tooltipPrefab, statusBar).GetComponent<Tooltip>();
        canvas = GetComponentInParent<Canvas>();
        fieldPanel = canvas.GetComponentInChildren<FieldManager>();
        rectTransform = GetComponent<RectTransform>();
            
        originPosition = transform.position;
        originLocalRotation = rectTransform.localRotation;

        tooltip.gameObject.SetActive(false);
        SetPotionData();
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable)
            return;

        tooltip.gameObject.SetActive(false);
        
        DOTween.Sequence()
            .Join(rectTransform.DOScale(0.5f, 0.2f))
            .Join(rectTransform.DOMove(eventData.position, 0))
            .Join(rectTransform.DOLocalRotate(new Vector3(0, 0, 30f), 0.2f));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable)
            return;
            
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        ICardTarget target = GetTargetUnderMouse();

        if (target != null)
        {
            if (hoveredTarget != target)
            {
                if (hoveredTarget != null)
                    fieldPanel.SetHighlight(targetType, hoveredTarget, false);

                hoveredTarget = target;

                if (hoveredTarget.CanApply(targetType))
                    fieldPanel.SetHighlight(targetType, hoveredTarget, true);
            }
        }
        else if (hoveredTarget != null)
        {
            fieldPanel.SetHighlight(targetType, hoveredTarget, false);
            hoveredTarget = null;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable)
            return;
            
        if (hoveredTarget != null && hoveredTarget.CanApply(targetType))
        {
            tooltip.gameObject.SetActive(false);
            fieldPanel.SetHighlight(targetType, hoveredTarget, false);
            fieldPanel.ApplyEffect(targetType, null, hoveredTarget, potionName);

            hoveredTarget = null;
            isActive = false;
            image.color = new Color32(0, 0, 0, 0);
            
            PlayerPrefs.SetString($"Potion{slotCount}", null);
        }
        else
        {
            DOTween.Sequence()
                .Join(rectTransform.DOScale(1f, 0.1f))
                .Join(rectTransform.DOMove(originPosition, 0.1f))
                .Join(rectTransform.DOLocalRotateQuaternion(originLocalRotation, 0.1f))
                .SetEase(Ease.OutQuad)
                .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(statusBar.GetComponent<RectTransform>()));
        }
    }

    public void SetPotionData()
    {
        string name = PlayerPrefs.GetString($"Potion{slotCount}");
        
        if (name.Length > 0)
        {
            var potion = db.GetPotion(name);

            image.sprite = potion.potionImage;
            image.color = Color.white;
            potionName = potion.name;
            targetType = potion.targetType;
            isActive = true;

            tooltip.gameObject.SetActive(true);
            tooltip.transform.position = transform.position + new Vector3(115f, -20f, 0);
            tooltip.SetTooltipData(potion.potionName, potion.description);
            tooltip.gameObject.SetActive(false);
        }
    }

    private ICardTarget GetTargetUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        canvas.GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

        foreach (var result in results)
        {
            var cardTarget = result.gameObject.GetComponentInParent<ICardTarget>();

            if (cardTarget != null)
                return cardTarget;
        }
        return null;
    }
}