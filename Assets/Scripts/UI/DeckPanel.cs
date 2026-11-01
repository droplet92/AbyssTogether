using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button okButton;

    private DeckManager deckManager;

    void OnEnable()
    {
        ScrollToTop();
    }
    void Start()
    {
        CreateCards();
        ScrollToTop();
        okButton.onClick.AddListener(ClosePanel);
    }
    void Update()
    {
        scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y * 0.1f;
    }

    public void Initialize(DeckManager deckManager)
    {
        this.deckManager = deckManager;
    }

    private void CreateCards()
    {
        foreach (var cardData in deckManager.CardList)
        {
            var cardUI = Instantiate(cardPrefab, content).GetComponent<CardUI>();
            cardUI.SetCardData(cardData, null);
            cardUI.Exhibit();
        }
    }

    private void ScrollToTop()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
