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
        for (int i = 0; i < deckManager.CardList.Count; i++)
        {
            var cardUI = Instantiate(cardPrefab, content).GetComponent<CardUI>();
            cardUI.SetCardData(deckManager.CardList[i], null);
            cardUI.Exhibit();

            int index = i;
            cardUI.AddTeamTalkEvent(() => SelectCard(cardUI, index));
        }
    }
    void Update()
    {
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
    private void SelectCard(CardUI card, int index)
    {
        selected?.SetOX(!isRemoveCardPhase, false);
        selected = card;
        selected.SetOX(!isRemoveCardPhase, true);

        if (isRemoveCardPhase) removeIndex = index;
        else copyIndex = index;
    }
    private void Confirm()
    {
        if (isRemoveCardPhase && removeIndex >= 0)
        {
            UpdatePanelToCopy();
        }
        else if (copyIndex >= 0)
        {
            OverwriteCard();
            gameObject.SetActive(false);
            campManager.ExitCamp();
        }
    }
    private void UpdatePanelToCopy()
    {
        selected = null;
        isRemoveCardPhase = false;
        removeString.StringChanged -= OnLabelChanged;
        copyString.StringChanged += OnLabelChanged;
        copyString.RefreshString();
    }
    private void OverwriteCard()
    {
        var data = PlayerPrefs.GetString("Deck");
        Debug.Assert(data.Length > 0, "Empty deck");

        var deck = JsonUtility.FromJson<DeckDataJson>(data);
        var copy = deck.cards[copyIndex];
        deck.cards.RemoveAt(removeIndex);
        deck.cards.Insert(removeIndex, copy);
        PlayerPrefs.SetString("Deck", JsonUtility.ToJson(deck));
    }
}
