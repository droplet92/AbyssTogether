using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardDatabase db;
    [SerializeField] private List<CardData> hand = new List<CardData>();

    private List<CardData> cardList = new List<CardData>();
    private List<CardData> playDeck = new List<CardData>();

    private void Start()
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
            };
            temp.cards.Add("Attack");
            temp.cards.Add("Attack");
            temp.cards.Add("AttackAll");
            temp.cards.Add("Shield");
            temp.cards.Add("Shield");
            temp.cards.Add("ShieldAll");
            temp.cards.Add("Heal");
            temp.cards.Add("Heal");
            temp.cards.Add("HealAll");
            temp.cards.Add("Buff");
            temp.cards.Add("Debuff");
            temp.cards.Add("Skill");
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
    public void PlayCard(CardData card)
    {
        if (hand.Contains(card))
        {
            // 카드 효과 적용 로직 등
            // ...
            hand.Remove(card);
        }
    }
    public void ClearHand()
    {
        hand.Clear();
    }
}
