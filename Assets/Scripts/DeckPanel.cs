using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckPanel : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button okButton;

    private List<CardData> cardList;

    void OnEnable()
    {
        cardList = deckManager.GetCardList();
        okButton.onClick.AddListener(ClosePanel);
        Initialize();
    }
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y * 0.1f;
    }

    public void Initialize()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject newCardObj = Instantiate(cardPrefab, content);
            CardUI cardUI = newCardObj.GetComponent<CardUI>();
            
            cardUI.SetCardData(cardList[i], null);
            cardUI.Exhibit();
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
