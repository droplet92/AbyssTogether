using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private HandUI hand;
    [SerializeField] private List<Character> characterList;
    [SerializeField] private List<Monster> monsterList;

    private int itemAttack = 0;
    private int itemDefense = 0;
    private int itemResistDebuff = 0;
    private int itemHealerAttack = 0;

    void Start()
    {
        UpdateItemEffects();
        monsterList.RemoveAll((Monster monster) => monster.IsDead());
    }
    void Update()
    {
        UpdateItemEffects();
    }
    public void SetHighlight(string targetType, ITarget target, bool isActive)
    {
        if (!targetType.EndsWith("All"))
        {
            target.SetHighlight(isActive);
        }
        else if (targetType == "CharacterAll")
        {
            foreach (var character in characterList)
                character.SetHighlight(isActive);
        }
        else if (targetType == "MonsterAll")
        {
            foreach (var monster in monsterList)
                monster.SetHighlight(isActive);
        }
    }
    public void ApplyEffect(string targetType, object from, ITarget target, string functionName)
    {
        Type type = typeof(FieldManager);
        MethodInfo methodInfo = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (from == null)
            from = characterList;

        if (targetType == "Character")
        {
            methodInfo.Invoke(this, new object[]{ from, target });
        }
        else if (targetType == "Monster")
        {
            methodInfo.Invoke(this, new object[]{ target });
        }
        else if (targetType == "CharacterAll")
        {
            methodInfo.Invoke(this, Array.Empty<object>());
        }
        else if (targetType == "MonsterAll")
        {
            methodInfo.Invoke(this, Array.Empty<object>());
        }
    }

    private void UpdateItemEffects()
    {
        var nextItemDefense = !characterList[0].IsDead() ? PlayerPrefs.GetInt("ItemNecklace") : 0;
        var nextItemAttack = !characterList[1].IsDead() ? PlayerPrefs.GetInt("ItemSword") : 0;
        var nextItemResistDebuff = !characterList[2].IsDead() ? PlayerPrefs.GetInt("ItemRing") : 0;
        var nextItemHealerAttack = !characterList[3].IsDead() ? PlayerPrefs.GetInt("ItemMagicBook") : 0;
    
        bool hasBuffChanged = itemDefense != nextItemDefense
            || itemAttack != nextItemAttack
            || itemHealerAttack != nextItemHealerAttack;

        itemDefense = nextItemDefense;
        itemAttack = nextItemAttack;
        itemResistDebuff = nextItemResistDebuff;
        itemHealerAttack = nextItemHealerAttack;

        if (hasBuffChanged)
            UpdateCharacterBuffs();
    }
    private void UpdateCharacterBuffs()
    {
        characterList[3].buffs.UpdateContents(itemHealerAttack, 0);
        foreach (var character in characterList)
            character.buffs.UpdateContents(itemAttack, itemDefense);
    }

    // Card effects
    private void Attack(Monster target)
    {
        var swordsman = characterList[1];
        int attack = swordsman.Attack + itemAttack;

        swordsman.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(math.min(target.Defense - attack, 0));
    }
    private void AttackAll()
    {
        var swordsman = characterList[1];
        int attack = swordsman.Attack + itemAttack;

        swordsman.AnimateAttack();

        foreach (var monster in monsterList)
        {
            monster.AnimateHit();
            monster.UpdateHealth(math.min(monster.Defense - attack, 0));
        }
    }
    private void Shield(object _, Character target)
    {
        var warrior = characterList[0];
        int defense = warrior.Attack + itemDefense;
        
        target.AnimateSpell();
        target.UpdateDefense(defense);
    }
    private void ShieldAll()
    {
        var warrior = characterList[0];
        int defense = warrior.Attack + itemDefense;
        
        foreach (var character in characterList)
        {
            character.AnimateSpell();
            character.UpdateDefense(defense);
        }
    }
    private void Buff(object _, Character target)
    {
        target.AnimateSpell();
        target.UpdateAttack(3);
    }
    private void Debuff(Monster target)
    {
        var magician = characterList[2];

        magician.AnimateSpell();
        target.AnimateHit();
        target.UpdateAttack(-1);
    }
    private void Skill(Monster target)
    {
        var magician = characterList[2];
        int attack = magician.Attack + itemAttack;

        magician.AnimateSpell();
        target.AnimateHit();
        target.UpdateHealth(math.min(target.Defense - attack, 0));
    }
    private void Heal(object _, Character target)
    {
        var healer = characterList[3];
        int attack = healer.Attack + itemAttack + itemHealerAttack;

        target.AnimateSpell();
        target.UpdateHealth(attack);
    }
    private void HealAll()
    {
        var healer = characterList[3];
        int attack = healer.Attack + itemAttack + itemHealerAttack;
        
        foreach (var character in characterList)
        {
            character.AnimateSpell();
            character.UpdateHealth(attack);
        }
    }

    // Monster actions
    private void ActionAttack(Monster monster, Character target)
    {
        int defense = target.Defense + itemDefense;

        monster.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(math.min(defense - monster.Attack, 0));
    }
    private void ActionDoubleAttack(Monster monster, Character target)
    {
        int defense = target.Defense + itemDefense;

        monster.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(math.min(defense - monster.Attack * 2, 0));
    }
    private void ActionShieldAttack(Monster monster, Character target)
    {
        int defense = target.Defense + itemDefense;

        monster.AnimateAttack();
        monster.UpdateDefense(6);
        target.AnimateHit();
        target.UpdateHealth(math.min(defense - monster.Attack, 0));
    }
    private void ActionShield(Monster monster, object _)
    {
        monster.AnimateSpell();
        monster.UpdateDefense(6);
    }
    private void ActionBuffAttack(Monster monster, object _)
    {
        monster.AnimateSpell();
        monster.UpdateAttack(3);
    }
    private void ActionDodgeAttack(Monster monster, object _)
    {
        monster.AnimateSpell();
        monster.UpdateDefense(99);
    }
    private void ActionDebuffAttack(Monster monster, Character target)
    {
        int debuff = -3 + itemResistDebuff;

        monster.AnimateSpell();
        target.AnimateHit();
        target.UpdateAttack(debuff);
    }
    private void ActionDebuffDraw1(Monster monster, object _)
    {
        int debuff = 1 - itemResistDebuff;

        monster.AnimateSpell();
        turnManager.ReduceDraw(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionDebuffDraw2(Monster monster, object _)
    {
        int debuff = 2 - itemResistDebuff;

        monster.AnimateSpell();
        turnManager.ReduceDraw(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionFreezeHand1(Monster monster, object _)
    {
        int debuff = 1 - itemResistDebuff;

        monster.AnimateSpell();
        hand.SetFreeze(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionFreezeHand2(Monster monster, object _)
    {
        int debuff = 2 - itemResistDebuff;

        monster.AnimateSpell();
        hand.SetFreeze(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }

    // Potion effects
    private void BluePotion()
    {
        var healer = characterList[3];

        healer.AnimateSpell();
        healer.UpdateAttack(1);
    }
    private void GreenPotion(object _, Character target)
    {
        target.AnimateSpell();
        target.UpdateDefense(2);
    }
    private void Gem(object _, Character target)
    {
        target.AnimateSpell();
        target.UpdateDefense(99);
    }
    private void Mushroom(object _, Character target)
    {
        target.AnimateSpell();
        target.UpdateAttack(2);
    }
    private void Twig(Monster target)
    {
        target.AnimateHit();
        target.UpdateAttack(-2);
    }
}
