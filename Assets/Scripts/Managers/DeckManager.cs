using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabase db;

    public List<CardData> CardList { get; } = new List<CardData>();
    private List<CardData> playDeck = new List<CardData>();
    private List<CardData> hand = new List<CardData>();

    public void Initialize()
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
        foreach (var cardName in SaveData.LoadDeck())
            CardList.Add(db.GetCard(cardName));
    }
}
