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
}
