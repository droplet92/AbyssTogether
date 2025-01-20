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
            .Join(card.DOMove(grave.position, 0.1f))
            .Join(card.transform.DOScale(0.5f, 0.1f))
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                {
                    handCards.Remove(card);
                    card.gameObject.SetActive(false);
                });
    }
    public IEnumerator DiscardAll()
    {
        foreach (var card in handCards)
        {
            Sequence seq = DOTween.Sequence()
                .Join(card.DOMove(grave.position, 0.1f))
                .Join(card.DOScale(0.5f, 0.1f))
                .SetEase(Ease.OutQuad);

            yield return seq.WaitForCompletion();

            card.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ClearHand()
    {
        handCards.Clear();
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
            onlyCard.DOAnchorPos(center, 0.3f).SetEase(Ease.OutQuad);
            onlyCard.DOLocalRotateQuaternion(Quaternion.identity, 0.3f);
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

            RectTransform cardRect = handCards[i];
            DOTween.Sequence()
                .Join(cardRect.DOAnchorPos(new Vector2(x, y), 0.3f))
                .Join(cardRect.DOLocalRotate(new Vector3(0, 0, angle + 180f), 0.3f))
                .SetEase(Ease.InQuad);
        }
    }
}
