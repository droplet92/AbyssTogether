using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;
using System;

public class CardUI : AutoFieldValidator, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image cardFrame;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardContentFrame;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardNameShadowText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private Image O;
    [SerializeField] private Image X;

    public CardData CardData { get; private set; }

    private Character owner;
    private int itemAttack = 0;
    private LocalizedString localizedDescription;
    private string targetType;
    private Canvas canvas;
    private FieldManager fieldPanel;
    private HandUI handPanel;
    private RectTransform rectTransform;
    private Vector3 originPos;
    private Vector2 originAnchorPos;
    private Vector3 originLocalRotation;
    private int originSiblingIndex;
    private ITarget hoveredTarget;
    private bool isFreeze = false;
    private bool uiOnly = false;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        fieldPanel = canvas.GetComponentInChildren<FieldManager>();
        handPanel = GetComponentInParent<HandUI>();
        rectTransform = GetComponent<RectTransform>();
    }
    void OnDestroy()
    {
        localizedDescription.StringChanged -= OnStringChanged;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging || uiOnly) return;

        originSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        DOTween.Sequence()
            .Join(rectTransform.DOScale(2f, 0.2f))
            .Join(rectTransform.DOLocalMoveY(360f, 0.2f))
            .Join(rectTransform.DOLocalRotate(Vector3.zero, 0.2f));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging || uiOnly) return;

        transform.SetSiblingIndex(originSiblingIndex);
        
        DOTween.Sequence()
            .Join(rectTransform.DOScale(1f, 0.2f))
            .Join(rectTransform.DOMove(originPos, 0.2f))
            .Join(rectTransform.DOAnchorPos(originAnchorPos, 0.2f))
            .Join(rectTransform.DOLocalRotate(originLocalRotation, 0.2f));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isFreeze) return;

        DOTween.Sequence()
            .Join(rectTransform.DOScale(0.5f, 0.2f))
            .Join(rectTransform.DOMove(eventData.position, 0))
            .Join(rectTransform.DOLocalRotate(new Vector3(0, 0, 30f), 0.2f));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isFreeze) return;
            
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        ITarget target = ITargetUtil.GetTargetUnderMouse(canvas);
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
        if (isFreeze) return;
            
        if (hoveredTarget?.CanApply(targetType) ?? false)
        {
            fieldPanel.SetHighlight(targetType, hoveredTarget, false);
            fieldPanel.ApplyEffect(targetType, null, hoveredTarget, cardNameText.text);
            hoveredTarget = null;
            
            Exhibit();
            handPanel.Discard(rectTransform);
        }
        else
        {
            transform.SetSiblingIndex(originSiblingIndex);

            DOTween.Sequence()
                .Join(rectTransform.DOScale(1f, 0.1f))
                .Join(rectTransform.DOMove(originPos, 0.1f))
                .Join(rectTransform.DOAnchorPos(originAnchorPos, 0.1f))
                .Join(rectTransform.DOLocalRotate(originLocalRotation, 0.1f))
                .SetEase(Ease.OutQuad)
                .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(handPanel.GetComponent<RectTransform>()));
        }
    }
    public void SetCardData(CardData data, Character cardOwner)
    {
        CardData = data;
        owner = cardOwner;
        cardFrame.sprite = data.cardFrame;
        cardImage.sprite = data.cardImage;
        cardContentFrame.sprite = data.cardContentFrame;
        cardNameText.text = data.cardName;
        cardNameShadowText.text = data.cardName;
        targetType = data.targetType;

        localizedDescription = data.description;
        localizedDescription.StringChanged += OnStringChanged;
        localizedDescription.RefreshString();

        if (owner == null) return;
        if (!owner.IsDead())
        {
            itemAttack = PlayerPrefs.GetInt("ItemSword");

            if (owner.name == "Healer")
                itemAttack += PlayerPrefs.GetInt("ItemMagicBook");

            localizedDescription.RefreshString();
        }
        owner.attackChanged += () => localizedDescription.RefreshString();
    }
    public void SetCardOrigin(Vector3 parentPos, Vector2 anchorPos, Vector3 localRotation)
    {
        originPos = parentPos;
        originAnchorPos = anchorPos;
        originLocalRotation = localRotation;
    }
    public void Freeze()
    {
        isFreeze = true;
        cardFrame.color = Color.black;
    }
    public void Exhibit()
    {
        isFreeze = true;
        uiOnly = true;
    }
    public void Disable()
    {
        Exhibit();
        cardFrame.color = Color.black;
    }
    public void AddTeamTalkEvent(Action selectCard)
    {
        var eventTrigger = gameObject.AddComponent<EventTrigger>();
        var clickEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick
        };
        clickEntry.callback.AddListener((data) => selectCard());
        eventTrigger.triggers.Add(clickEntry);
    }
    public void SetOX(bool isO, bool activate)
    {
        if (isO) O.gameObject.SetActive(activate);
        else X.gameObject.SetActive(activate);
    }

    private void OnStringChanged(string value)
    {
        cardDescriptionText.text = value.FormatSmart(GetCardValue());
    }
    private int GetCardValue()
    {
        if (owner != null) return owner.Attack + itemAttack;
        if (cardNameText.text.StartsWith("Shield")) return 3;
        if (cardNameText.text.StartsWith("Attack")) return 4;
        if (cardNameText.text.StartsWith("Heal")) return 2;
        return 9;
    }
}
