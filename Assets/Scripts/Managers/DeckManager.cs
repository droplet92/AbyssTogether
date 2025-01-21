using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabase db;
    [SerializeField] private List<CardData> hand = new List<CardData>();

    private List<CardData> cardList = new List<CardData>();
    private List<CardData> playDeck = new List<CardData>();

    void Awake()
    {
        LoadDeck();
        ResetDeck();
        ShuffleDeck();
    }
    
    public List<CardData> GetCardList()
    {
        return cardList;
    }
    public void ResetDeck()
    {
        playDeck.Clear();
        playDeck.AddRange(cardList);
    }
    public void ShuffleDeck()
    {
        for (int i = 0; i < playDeck.Count; i++)
        {
            CardData temp = playDeck[i];
            int randomIndex = Random.Range(i, playDeck.Count);
            playDeck[i] = playDeck[randomIndex];
            playDeck[randomIndex] = temp;
        }
    }
    public CardData DrawCard()
    {
        if (playDeck.Count == 0)
            return null;

        CardData drawnCard = playDeck[0];
        playDeck.RemoveAt(0);
        hand.Add(drawnCard);
        
        return drawnCard;
    }
    
    private void LoadDeck()
    {
        var data = PlayerPrefs.GetString("Deck");

        if (data.Length == 0)
        {
            DeckDataJson newData = new DeckDataJson()
            {
                cards = new List<string>()
                {
                    "Attack", "Attack", "AttackAll",
                    "Shield", "Shield", "ShieldAll",
                    "Heal", "Heal", "HealAll",
                    "Buff", "Debuff", "Skill"
                }
            };
            data = JsonUtility.ToJson(newData);
            PlayerPrefs.SetString("Deck", data);
        }
        var deck = JsonUtility.FromJson<DeckDataJson>(data);

        foreach (var cardName in deck.cards)
            cardList.Add(db.GetCard(cardName));
    }
}
