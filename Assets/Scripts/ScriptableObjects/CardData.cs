using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
public class CardData : ScriptableObject
{
    public Sprite cardFrame;
    public Sprite cardImage;
    public Sprite cardContentFrame;
    public string cardName;
    public LocalizedString description;
    public string targetType;
    public PartyMember owner;
    [Tooltip("Value shown when the card is not held by a party member, such as in the deck view.")]
    public int baseValue;
}
