using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Monster System/Monster")]
public class MonsterData : ScriptableObject
{
    public Sprite body;
    public Sprite bodyHightlight;
    public int health;
    public int attack;
    public int defense;
    public List<string> actions;
}
