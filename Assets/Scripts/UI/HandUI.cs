using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class HandUI : AutoFieldValidator
{
    [SerializeField] private List<Character> characters;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform deckTransform;
    [SerializeField] private RectTransform grave;
    
    private int nFreeze = 0;
    private List<RectTransform> handCards = new List<RectTransform>();
    private static List<string> cardPrefix = new List<string>() { "Shield", "Attack", null, "Heal" };

    public void DrawCard()
    {
        CardData drawnCard = deckManager.DrawCard();
        deckManager.ResetDeck();
        deckManager.ShuffleDeck();
        drawnCard = deckManager.DrawCard();

        var cardUI = Instantiate(cardPrefab, transform).GetComponent<CardUI>();
        cardUI.transform.localScale = Vector3.one / 2;
        cardUI.transform.position = deckTransform.position;

        for (int i = 0; i < cardPrefix.Count; i++)
        {
            bool isCharacters = cardPrefix[i] == null || drawnCard.cardName.StartsWith(cardPrefix[i]);
            if (isCharacters)
            {
                if (characters[i].IsDead())
                    cardUI.Disable();

                cardUI.SetCardData(drawnCard, characters[i]);
            }
        }
        DOTween.Sequence()
            .Join(cardUI.transform.DOScale(1f, 0.5f))
            .Join(cardUI.transform.DOMove(transform.position, 0.5f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (handCards.Count < nFreeze)
                    cardUI.Freeze();

                handCards.Add(cardUI.GetComponent<RectTransform>());
                ArrangeHand();
            });
    }
    public void Discard(RectTransform card)
    {
        DOTween.Sequence()
            .Join(card.DOScale(0.5f, 0.1f))
            .Join(card.DOLocalMove(grave.localPosition, 0.1f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                handCards.Remove(card);
                Destroy(card.gameObject);
            });
    }
    public IEnumerator DiscardAll()
    {
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
        yield break;
    }
    public void SetFreeze(int n)
    {
        nFreeze = n;
    }

    private void ArrangeHand()
    {
        if (handCards.Count == 0) return;
        const float totalArc = 45f;   // degree 45~135
        const float radius = 1000f;
        Vector2 center = Vector2.zero;

        if (handCards.Count == 1)
        {
            RectTransform onlyCard = handCards[0];
            onlyCard.GetComponent<CardUI>().SetCardOrigin(transform.position, center, Vector3.zero);
            
            DOTween.Sequence()
                .Join(onlyCard.DOAnchorPos(center, 0.3f))
                .Join(onlyCard.DOLocalRotateQuaternion(Quaternion.identity, 0.3f))
                .SetEase(Ease.OutQuad);
        }
        else
        {
            const float startAngle = -totalArc / 2 - 135f;
            float angleStep = totalArc / (handCards.Count - 1);

            for (int i = 0; i < handCards.Count; i++)
            {
                float angle = startAngle - (angleStep * i);
                float rad = angle * Mathf.Deg2Rad;
                float x = center.x + radius * Mathf.Sin(rad);
                float y = center.y - radius - radius * Mathf.Cos(rad);
                var anchorPos = new Vector2(x, y);
                var localRotate = new Vector3(0, 0, angle + 180f);

                RectTransform cardRect = handCards[i];
                cardRect.GetComponent<CardUI>().SetCardOrigin(transform.position, anchorPos, localRotate);
                
                DOTween.Sequence()
                    .Join(cardRect.DOAnchorPos(anchorPos, 0.3f))
                    .Join(cardRect.DOLocalRotate(localRotate, 0.3f))
                    .SetEase(Ease.InQuad);
            }
        }
    }
}
