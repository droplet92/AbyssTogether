using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TeamTalkPanel : MonoBehaviour
{
    [SerializeField] private CampSceneManager campManager;
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        scrollRect.verticalNormalizedPosition = 1f;
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
        var deck = SaveData.LoadDeck();
        var copy = deck[copyIndex];
        deck.RemoveAt(removeIndex);
        deck.Insert(removeIndex, copy);
        SaveData.SaveDeck(deck);
    }
}
