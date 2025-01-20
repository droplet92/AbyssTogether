using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "PotionDatabase", menuName = "Game/PotionDatabase")]
public class PotionDatabase : ScriptableObject
{
    [SerializeField] private List<PotionData> potionDataList;

    public PotionData GetPotion(string potionName)
    {
        return potionDataList.FirstOrDefault(x => x.name == potionName);
    }
    public PotionData GetRandomPotion()
    {
        int randomIndex = Random.Range(0, potionDataList.Count);

        return potionDataList[randomIndex];
    }
}
