using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class HandUI : MonoBehaviour
{
    [SerializeField] private List<Character> characters;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform deckTransform;
    [SerializeField] private RectTransform grave;
    
    private List<RectTransform> handCards = new List<RectTransform>();
    private Dictionary<RectTransform, DeckCard> deckCards = new Dictionary<RectTransform, DeckCard>();
    public void DrawCard()
    {
        DeckCard drawnCard = deckManager.Deck.Draw();
        var cardUI = Instantiate(cardPrefab, transform).GetComponent<CardUI>();
        cardUI.transform.localScale = Vector3.one / 2;
        cardUI.transform.position = deckTransform.position;

        var owner = characters[(int)drawnCard.Data.owner];
        cardUI.SetCardData(drawnCard.Data, owner);
        if (owner.IsDead())
            cardUI.Disable();

        bool freeze = deckManager.Deck.ConsumeFreeze();
        var cardRect = cardUI.GetComponent<RectTransform>();
        deckCards[cardRect] = drawnCard;

        DOTween.Sequence()
            .Join(cardUI.transform.DOScale(1f, 0.5f))
            .Join(cardUI.transform.DOMove(transform.position, 0.5f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (freeze)
                    cardUI.Freeze();

                handCards.Add(cardRect);
                ArrangeHand();
            });
    }
    public void Discard(RectTransform card)
    {
        handCards.Remove(card);
        deckManager.Deck.Play(deckCards[card]);
        deckCards.Remove(card);
        ArrangeHand();

        DOTween.Sequence()
            .Join(card.DOScale(0.5f, 0.1f))
            .Join(card.DOLocalMove(grave.localPosition, 0.1f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Destroy(card.gameObject));
    }
    public IEnumerator DiscardAll()
    {
        foreach (var card in handCards)
            card.GetComponent<CardUI>().Exhibit();

        deckManager.Deck.DiscardHand();

        foreach (var card in handCards)
        {
            Sequence seq = DOTween.Sequence()
                .Join(card.DOScale(0.5f, 0.1f))
                .Join(card.DOLocalMove(grave.localPosition, 0.1f))
                .SetEase(Ease.OutQuad);
            yield return seq.WaitForCompletion();

            Destroy(card.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        handCards.Clear();
        deckCards.Clear();
        yield break;
    }
    public void SetFreeze(int n)
    {
        deckManager.Deck.SetFreeze(n);
    }

    private void ArrangeHand()
    {
        const float angleStep = 15f;
        const float radius = 1000f;
        float startAngle = -180f + angleStep * (handCards.Count - 1) / 2;

        for (int i = 0; i < handCards.Count; i++)
        {
            float angle = startAngle - angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            var anchorPos = new Vector2(radius * Mathf.Sin(rad), -radius - radius * Mathf.Cos(rad));
            var localRotate = new Vector3(0, 0, angle + 180f);

            handCards[i].GetComponent<CardUI>().MoveToHandSlot(transform.position, anchorPos, localRotate);
        }
    }
}
