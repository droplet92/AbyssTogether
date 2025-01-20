using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamTalkPanelPanel : MonoBehaviour
{
    [SerializeField] private CampTeamTalkOkButton okButton;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardPrefab;

    private List<CardData> cardList;

    void OnEnable()
    {
        cardList = deckManager.GetCardList();
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
            cardUI.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            int index = i;
            entry.callback.AddListener((data) => okButton.SelectCard(cardUI, index));
            cardUI.GetComponent<EventTrigger>()
                .triggers
                .Add(entry);
        }
    }
}
