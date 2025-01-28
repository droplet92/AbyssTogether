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
    
    private List<RectTransform> handCards = new List<RectTransform>();
    private int nFreeze = 0;

    public void DrawCard()
    {
        CardData drawnCard = deckManager.DrawCard();

        if (!drawnCard)
        {
            deckManager.ResetDeck();
            deckManager.ShuffleDeck();
            drawnCard = deckManager.DrawCard();
        }
        GameObject newCardObj = Instantiate(cardPrefab, transform);
        CardUI cardUI = newCardObj.GetComponent<CardUI>();
        newCardObj.transform.localScale = Vector3.one / 2;
        
        if (drawnCard.cardName == "Shield" || drawnCard.cardName == "ShieldAll")
        {
            if (!characters[0].gameObject.activeSelf)
                cardUI.Disable();

            cardUI.SetCardData(drawnCard, characters[0]);
        }
        else if (drawnCard.cardName == "Attack" || drawnCard.cardName == "AttackAll")
        {
            if (!characters[1].gameObject.activeSelf)
                cardUI.Disable();

            cardUI.SetCardData(drawnCard, characters[1]);
        }
        else if (drawnCard.cardName == "Heal" || drawnCard.cardName == "HealAll")
        {
            if (!characters[3].gameObject.activeSelf)
                cardUI.Disable();
            
            cardUI.SetCardData(drawnCard, characters[3]);
        }
        else
        {
            if (!characters[2].gameObject.activeSelf)
                cardUI.Disable();

            cardUI.SetCardData(drawnCard, characters[2]);
        }
        RectTransform cardRect = newCardObj.GetComponent<RectTransform>();
        cardRect.position = deckTransform.position;

        DOTween.Sequence()
            .Join(cardRect.DOScale(1f, 0.5f))
            .Join(cardRect.DOMove(transform.position, 0.5f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                {
                    if (handCards.Count < nFreeze)
                        cardUI.Freeze();

                    handCards.Add(cardRect);
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
        int count = handCards.Count;

        if (count == 0)
            return;

        float totalArc = 45f;   // degree 45~135
        float radius = 1000f;
        Vector2 center = Vector2.zero;

        if (count == 1)
        {
            RectTransform onlyCard = handCards[0];
            onlyCard.GetComponent<CardUI>().SetCardOrigin(transform.position, center, Vector3.zero);
            DOTween.Sequence()
                .Join(onlyCard.DOAnchorPos(center, 0.3f))
                .Join(onlyCard.DOLocalRotateQuaternion(Quaternion.identity, 0.3f))
                .SetEase(Ease.OutQuad);
            return;
        }
        float angleStep = totalArc / (count - 1);
        float startAngle = -totalArc / 2 - 135f;

        for (int i = 0; i < count; i++)
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
