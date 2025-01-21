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
        Initialize();
        ResetDeck();
        ShuffleDeck();
    }
    
    private void Initialize()
    {
        var path = Path.Combine(Application.dataPath, "Deck.json");

        if (!File.Exists(path))
        {
            DeckDataJson temp = new DeckDataJson()
            {
                cards = new List<string>()
                {
                    "Attack", "Attack", "AttackAll",
                    "Shield", "Shield", "ShieldAll",
                    "Heal", "Heal", "HealAll",
                    "Buff", "Debuff", "Skill"
                }
            };
            File.WriteAllText(path, JsonUtility.ToJson(temp));
        }
        var data = File.ReadAllText(path);
        var deck = JsonUtility.FromJson<DeckDataJson>(data);

        foreach (var cardName in deck.cards)
            cardList.Add(db.GetCard(cardName));
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
}
