using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image cardFrame;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardContentFrame;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardNameShadowText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private Image O;
    [SerializeField] private Image X;

    private Character owner;
    private int itemAttack = 0;
    private LocalizedString localizedDescription;
    private string targetType;
    private Canvas canvas;
    private FieldManager fieldPanel;
    private HandUI handPanel;
    private RectTransform currentRectTransform;
    private float originLocalY;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private int originSiblingIndex;
    private ICardTarget hoveredTarget;
    private bool isFreeze = false;
    private bool uiOnly = false;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        fieldPanel = canvas.GetComponentInChildren<FieldManager>();
        handPanel = GetComponentInParent<HandUI>();
        currentRectTransform = GetComponent<RectTransform>();
    }
    void OnDestroy()
    {
        localizedDescription.StringChanged -= OnStringChanged;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging || uiOnly)
            return;

        originSiblingIndex = transform.GetSiblingIndex();
        originPosition = currentRectTransform.position;
        originLocalY = transform.localPosition.y;
        originRotation = transform.localRotation;

        transform.SetAsLastSibling();

        DOTween.Sequence()
            .Join(transform.DOScale(2f, 0.2f))
            .Join(transform.DOLocalMoveY(360f, 0.2f))
            .Join(transform.DOLocalRotate(Vector3.zero, 0.2f));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging || uiOnly)
            return;

        transform.SetSiblingIndex(originSiblingIndex);
        
        DOTween.Sequence()
            .Join(transform.DOScale(1f, 0.2f))
            .Join(transform.DOLocalMoveY(originLocalY, 0.2f))
            .Join(transform.DOLocalRotateQuaternion(originRotation, 0.2f));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFreeze)
            return;

        DOTween.Sequence()
            .Join(transform.DOScale(0.5f, 0.2f))
            .Join(currentRectTransform.DOMove(eventData.position, 0))
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 30f), 0.2f));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isFreeze)
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
        if (isFreeze)
            return;
            
        if (hoveredTarget != null && hoveredTarget.CanApply(targetType))
        {
            fieldPanel.SetHighlight(targetType, hoveredTarget, false);
            fieldPanel.ApplyEffect(targetType, null, hoveredTarget, cardNameText.text);
            handPanel.Discard(currentRectTransform);

            hoveredTarget = null;
        }
        else
        {
            transform.SetSiblingIndex(originSiblingIndex);

            DOTween.Sequence()
                .Join(transform.DOScale(1f, 0.1f))
                .Join(currentRectTransform.DOMove(originPosition, 0.1f))
                .Join(transform.DOLocalMoveY(originLocalY, 0.1f))
                .Join(transform.DOLocalRotateQuaternion(originRotation, 0.1f))
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(handPanel.GetComponent<RectTransform>());
                });
        }
    }
    public void SetCardData(CardData data, Character cardOwner)
    {
        owner = cardOwner;
        cardFrame.sprite = data.cardFrame;
        cardImage.sprite = data.cardImage;
        cardContentFrame.sprite = data.cardContentFrame;
        cardNameText.text = data.cardName;
        cardNameShadowText.text = data.cardName;
        targetType = data.targetType;

        localizedDescription = data.description;
        localizedDescription.StringChanged += OnStringChanged;

        if (owner != null)
        {
            if (owner.gameObject.activeSelf)
            {
                itemAttack = PlayerPrefs.GetInt("ItemSword");

                if (owner.name == "Healer")
                    itemAttack += PlayerPrefs.GetInt("ItemMagicBook");
            }
            owner.attackChanged += () => localizedDescription.RefreshString();
        }
        localizedDescription.RefreshString();
    }
    public void Freeze()
    {
        isFreeze = true;
        cardFrame.color = Color.black;
    }
    public void Exhibit()
    {
        uiOnly = isFreeze = true;
    }
    public void Disable()
    {
        Exhibit();
        cardFrame.color = Color.black;
    }
    public void SetOX(bool isO, bool activate)
    {
        if (isO)
            O.gameObject.SetActive(activate);
        else
            X.gameObject.SetActive(activate);
    }

    private void OnStringChanged(string value)
    {
        if (owner != null)
            cardDescriptionText.text = value.FormatSmart(owner.Attack + itemAttack);

        else if (cardNameText.text == "Shield" || cardNameText.text == "ShieldAll")
            cardDescriptionText.text = value.FormatSmart(6);

        else if (cardNameText.text == "Attack" || cardNameText.text == "AttackAll")
            cardDescriptionText.text = value.FormatSmart(6);

        else if (cardNameText.text == "Heal" || cardNameText.text == "HealAll")
            cardDescriptionText.text = value.FormatSmart(4);

        else
            cardDescriptionText.text = value.FormatSmart(12);
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
