using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Equipment System/Equipment")]
public class EquipmentData : ScriptableObject
{
    public LocalizedString equipmentName;
    public Sprite equipmentImage;
    public LocalizedString description;
}
