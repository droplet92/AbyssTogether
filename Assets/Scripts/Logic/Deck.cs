using System;
using System.Collections.Generic;

public class Deck<T> where T : class
{
    private readonly Func<int, int, int> randomRange;
    private readonly List<T> drawPile = new List<T>();
    private readonly List<T> used = new List<T>();
    private readonly List<T> hand = new List<T>();
    private int freeze = 0;

    public IReadOnlyList<T> DrawPile => drawPile;
    public IReadOnlyList<T> Used => used;
    public IReadOnlyList<T> Hand => hand;

    // randomRange(min, max) returns an int in [min, max), like UnityEngine.Random.Range.
    public Deck(IEnumerable<T> cards, Func<int, int, int> randomRange)
    {
        this.randomRange = randomRange;
        drawPile.AddRange(cards);
        Shuffle();
    }

    // Draws the top card. When the draw pile is empty, the used cards are shuffled back in first.
    public T Draw()
    {
        if (drawPile.Count == 0)
        {
            drawPile.AddRange(used);
            Shuffle();
            used.Clear();
        }
        if (drawPile.Count == 0) return null;

        T card = drawPile[0];
        drawPile.RemoveAt(0);
        hand.Add(card);
        return card;
    }
    public void Play(T card)
    {
        if (hand.Remove(card))
            used.Add(card);
    }
    public void DiscardHand()
    {
        used.AddRange(hand);
        hand.Clear();
    }

    public void SetFreeze(int count)
    {
        freeze = count;
    }
    // Decides whether the next drawn card is frozen, spending one freeze if so.
    public bool ConsumeFreeze()
    {
        if (freeze <= 0) return false;

        freeze--;
        return true;
    }

    private void Shuffle()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            int randomIndex = randomRange(i, drawPile.Count);
            (drawPile[i], drawPile[randomIndex]) = (drawPile[randomIndex], drawPile[i]);
        }
    }
}
