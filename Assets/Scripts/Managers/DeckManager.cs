using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabase db;

    public List<CardData> CardList { get; } = new List<CardData>();
    private List<CardData> playDeck = new List<CardData>();
    private List<CardData> hand = new List<CardData>();

    void Awake()
    {
        LoadDeck();
        ResetDeck();
        ShuffleDeck();
    }
    
    public void ResetDeck()
    {
        playDeck.Clear();
        playDeck.AddRange(CardList);
    }
    public void ResetDeck(List<CardData> usedCards)
    {
        playDeck.Clear();
        playDeck.AddRange(usedCards);
    }
    public void ShuffleDeck()
    {
        for (int i = 0; i < playDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, playDeck.Count);
            CardData temp = playDeck[i];
            playDeck[i] = playDeck[randomIndex];
            playDeck[randomIndex] = temp;
        }
    }
    public CardData DrawCard()
    {
        if (playDeck.Count == 0) return null;
        
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
            CardList.Add(db.GetCard(cardName));
    }
}
