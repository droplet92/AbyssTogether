using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabase db;

    public List<CardData> CardList { get; } = new List<CardData>();
    public Deck<DeckCard> Deck { get; private set; }

    public void Initialize()
    {
        foreach (var cardName in SaveData.LoadDeck())
            CardList.Add(db.GetCard(cardName));

        Deck = new Deck<DeckCard>(CardList.Select(card => new DeckCard(card)), Random.Range);
    }
}
