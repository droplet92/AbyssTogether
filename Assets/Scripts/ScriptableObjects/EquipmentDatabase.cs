using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Game/EquipmentDatabase")]
public class EquipmentDatabase : ScriptableObject
{
    [SerializeField] private List<EquipmentData> equipmentDataList;

    public EquipmentData GetEquipment(string equipmentName)
    {
        return equipmentDataList.FirstOrDefault(x => x.name == equipmentName);
    }
    public EquipmentData GetRandomEquipment()
    {
        int randomIndex = Random.Range(0, equipmentDataList.Count);

        return equipmentDataList[randomIndex];
    }
}
