using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterDatabase db;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Monster monster1;
    [SerializeField] private Monster monster2;
    [SerializeField] private Monster monster3;
    
    private readonly List<List<string>> levelMap = new List<List<string>>
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

        monster1.gameObject.SetActive(true);
        monster1.Initialize(db.GetMonster(monsters[0]));

        if (monsters.Count >= 2)
        {
            monster2.gameObject.SetActive(true);
            monster2.Initialize(db.GetMonster(monsters[1]));
        }
        else
            monster2.gameObject.SetActive(false);

        if (monsters.Count == 3)
        {
            monster3.gameObject.SetActive(true);
            monster3.Initialize(db.GetMonster(monsters[2]));
        }
        else
            monster3.gameObject.SetActive(false);
    }
}
