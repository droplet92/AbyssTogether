using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Game/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    [SerializeField] private List<CardData> cardDataList;

    public CardData GetCard(string cardName)
    {
        return cardDataList.FirstOrDefault(x => x.name == cardName);
    }
}
