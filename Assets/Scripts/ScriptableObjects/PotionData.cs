using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Potion System/Potion")]
public class PotionData : ScriptableObject
{
    public LocalizedString potionName;
    public Sprite potionImage;
    public LocalizedString description;
    public string targetType;
}
