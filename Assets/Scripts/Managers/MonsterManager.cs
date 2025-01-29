using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterDatabase db;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private List<Monster> monsterSlots;
    
    private static List<List<string>> levelMap = new List<List<string>>
    {
        new List<string>{"Criven"},
        new List<string>{"Criven","Criven"},
        null,
        new List<string>{"CrivenKing"},
        new List<string>{"Slime","Slime"},
        new List<string>{"Slime","HeraklesBeetle","Slime"},
        null,
        new List<string>{"Slime","HeraklesBeetle","Del"},
        new List<string>{"Del","Del"},
        new List<string>{"Segel","Del"},
        null,
        new List<string>{"GiantGolem"},
        new List<string>{"AncientCrivenKing","AncientSlimeKing"},
        new List<string>{"Golem","IceMantle","Golem"},
        new List<string>{"JudgementSegel","JudgementSegel"},
        null,
        new List<string>{"HeavensDeadAngel"},
    };

    void Awake()
    {
        var level = PlayerPrefs.GetInt("level");
        var monsters = levelMap[level - 1];

        for (int i = 0; i < monsterSlots.Count; i++)
        {
            if (i < monsters.Count)
            {
                monsterSlots[i].gameObject.SetActive(true);
                monsterSlots[i].Initialize(db.GetMonster(monsters[i]));
            }
            else
            {
                monsterSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
