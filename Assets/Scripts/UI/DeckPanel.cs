using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : AutoFieldValidator
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button okButton;

    void OnEnable()
    {
        Initialize();
        okButton.onClick.AddListener(ClosePanel);
    }
    void Update()
    {
        scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y * 0.1f;
    }

    public void Initialize()
    {
        foreach (var cardData in deckManager.CardList)
        {
            var cardUI = Instantiate(cardPrefab, content).GetComponent<CardUI>();
            cardUI.SetCardData(cardData, null);
            cardUI.Exhibit();
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
