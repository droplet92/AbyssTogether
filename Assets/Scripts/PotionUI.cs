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
    private RectTransform currentRectTransform;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private ICardTarget hoveredTarget;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        fieldPanel = canvas.GetComponentInChildren<FieldManager>();
        currentRectTransform = GetComponent<RectTransform>();

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
            
        originPosition = currentRectTransform.position;
        originRotation = transform.localRotation;

        tooltip.gameObject.SetActive(false);
        
        DOTween.Sequence()
            .Join(transform.DOScale(0.5f, 0.2f))
            .Join(currentRectTransform.DOMove(eventData.position, 0))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 30f), 0.2f));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive || !isDraggable)
            return;
            
        currentRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

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
                .Join(transform.DOScale(1f, 0.1f))
                .Join(currentRectTransform.DOMove(originPosition, 0.1f))
                .Join(transform.DOLocalRotateQuaternion(originRotation, 0.1f))
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(statusBar.GetComponent<RectTransform>());
                });
        }
    }

    public void SetPotionData()
    {
        string name = PlayerPrefs.GetString($"Potion{slotCount}");
        
        if (name.Length > 0)
        {
            var potion = db.GetPotion(name);

            tooltip = Instantiate(tooltipPrefab, statusBar).GetComponent<Tooltip>();
            image.sprite = potion.potionImage;
            image.color = Color.white;
            potionName = potion.name;
            targetType = potion.targetType;
            isActive = true;

            tooltip.SetTooltipData(potion.potionName, potion.description);
            tooltip.gameObject.SetActive(false);

            var rectTransform = image.GetComponent<RectTransform>();
            tooltip.GetComponent<RectTransform>().position = rectTransform.position + new Vector3(230f, -25f, 0);
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