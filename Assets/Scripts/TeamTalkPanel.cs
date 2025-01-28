using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TeamTalkPanelPanel : AutoFieldValidator
{
    [SerializeField] private CampManager campManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TMP_Text label;
    [SerializeField] private LocalizedString removeString;
    [SerializeField] private LocalizedString copyString;
    [SerializeField] private Button okButton;

    private List<CardData> cardList;
    private CardUI selected;
    private bool isRemoveCardPhase = true;
    private int removeIndex = -1;
    private int copyIndex = -1;


    void Awake()
    {
        removeString.StringChanged += OnLabelChanged;
        removeString.RefreshString();
        
        okButton.onClick.AddListener(Confirm);
    }
    void OnEnable()
    {
        cardList = deckManager.GetCardList();
        
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject newCardObj = Instantiate(cardPrefab, content);
            CardUI cardUI = newCardObj.GetComponent<CardUI>();
            
            cardUI.SetCardData(cardList[i], null);
            cardUI.Exhibit();
            cardUI.AddComponent<EventTrigger>();

            int index = i;
            var entry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerClick
            };
            entry.callback.AddListener((data) => SelectCard(cardUI, index));
            cardUI.GetComponent<EventTrigger>()
                .triggers
                .Add(entry);
        }
    }
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y * 0.1f;
    }
    void OnDestroy()
    {
        copyString.StringChanged -= OnLabelChanged;
    }
    
    private void OnLabelChanged(string value)
    {
        label.text = value;
    }
    private void Confirm()
    {
        if (isRemoveCardPhase && removeIndex >= 0)
        {
            selected = null;
            isRemoveCardPhase = false;
            removeString.StringChanged -= OnLabelChanged;
            copyString.StringChanged += OnLabelChanged;
            copyString.RefreshString();
        }
        else if (copyIndex >= 0)
        {
            var data = PlayerPrefs.GetString("Deck");

            if (data.Length > 0)
            {
                var deck = JsonUtility.FromJson<DeckDataJson>(data);
                var copy = deck.cards[copyIndex];
                deck.cards.RemoveAt(removeIndex);
                deck.cards.Insert(removeIndex, copy);
                PlayerPrefs.SetString("Deck", JsonUtility.ToJson(deck));
            }
            gameObject.SetActive(false);
            campManager.ExitCamp();
        }
    }
    private void SelectCard(CardUI card, int index)
    {
        if (selected != null)
            selected.SetOX(!isRemoveCardPhase, false);

        selected = card;
        selected.SetOX(!isRemoveCardPhase, true);

        if (isRemoveCardPhase)
            removeIndex = index;
        else
            copyIndex = index;
    }
}
