using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class DeckTests
{
    private class Card
    {
        public string Name { get; }
        public Card(string name) { Name = name; }
    }

    // Always picks the lower bound, so shuffling keeps the original order.
    private static int NoShuffle(int min, int max) => min;

    private static List<Card> Cards(params string[] names) => names.Select(n => new Card(n)).ToList();

    [Test]
    public void Draw_TakesCardsFromTheTopIntoHand()
    {
        var cards = Cards("A", "B", "C");
        var deck = new Deck<Card>(cards, NoShuffle);

        Assert.AreSame(cards[0], deck.Draw());
        Assert.AreSame(cards[1], deck.Draw());
        CollectionAssert.AreEqual(new[] { cards[0], cards[1] }, deck.Hand);
        CollectionAssert.AreEqual(new[] { cards[2] }, deck.DrawPile);
    }

    [Test]
    public void Shuffle_SwapsEachPositionWithTheRandomIndex()
    {
        var cards = Cards("A", "B", "C");
        // Always pick the last index: A<->C, then B<->A(at 2) -> C, A, B
        var deck = new Deck<Card>(cards, (min, max) => max - 1);

        CollectionAssert.AreEqual(new[] { "C", "A", "B" }, deck.DrawPile.Select(c => c.Name));
    }

    [Test]
    public void Play_MovesTheExactCardToUsed_EvenWithDuplicates()
    {
        var cards = Cards("A", "B", "A");
        var deck = new Deck<Card>(cards, NoShuffle);
        deck.Draw(); deck.Draw(); deck.Draw();

        deck.Play(cards[2]);

        CollectionAssert.AreEqual(new[] { cards[0], cards[1] }, deck.Hand);
        CollectionAssert.AreEqual(new[] { cards[2] }, deck.Used);
    }

    [Test]
    public void DiscardHand_KeepsHandOrderInUsed()
    {
        var cards = Cards("A", "B", "C");
        var deck = new Deck<Card>(cards, NoShuffle);
        deck.Draw(); deck.Draw(); deck.Draw();
        deck.Play(cards[1]);

        deck.DiscardHand();

        CollectionAssert.AreEqual(new[] { cards[1], cards[0], cards[2] }, deck.Used);
        Assert.IsEmpty(deck.Hand);
    }

    [Test]
    public void Draw_ReshufflesUsedCardsWhenDrawPileIsEmpty()
    {
        var cards = Cards("A", "B");
        var deck = new Deck<Card>(cards, NoShuffle);
        deck.Draw(); deck.Draw();
        deck.DiscardHand();

        Card drawn = deck.Draw();

        Assert.AreSame(cards[0], drawn);
        Assert.IsEmpty(deck.Used);
        CollectionAssert.AreEqual(new[] { cards[1] }, deck.DrawPile);
    }

    [Test]
    public void Draw_ReturnsNullWhenNoCardsAreLeftAnywhere()
    {
        var deck = new Deck<Card>(Cards("A"), NoShuffle);
        deck.Draw();

        Assert.IsNull(deck.Draw());
    }

    [Test]
    public void Freeze_IsSpentOnePerCard()
    {
        var deck = new Deck<Card>(Cards("A"), NoShuffle);
        deck.SetFreeze(2);

        Assert.IsTrue(deck.ConsumeFreeze());
        Assert.IsTrue(deck.ConsumeFreeze());
        Assert.IsFalse(deck.ConsumeFreeze());
    }
}
