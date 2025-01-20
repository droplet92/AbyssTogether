using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Game/MonsterDatabase")]
public class MonsterDatabase : ScriptableObject
{
    [SerializeField] private List<MonsterData> monsterDataList;

    public MonsterData GetMonster(string monsterName)
    {
        return monsterDataList.FirstOrDefault(x => x.name == monsterName);
    }
}
