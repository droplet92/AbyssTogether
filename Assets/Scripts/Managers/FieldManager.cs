using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private HandUI hand;
    [SerializeField] private List<Character> characterList;
    [SerializeField] private List<Monster> monsterList;

    private ItemBonus bonus;
    private Dictionary<string, Action<object, ITarget>> effects;

    public event Action itemEffectsChanged = () => {};

    public void Initialize()
    {
        effects = CreateEffects();
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
    public void ApplyEffect(string targetType, object from, ITarget target, string effectName)
    {
        if (from == null)
            from = characterList;

        effects[effectName](from, target);
    }

    private void UpdateItemEffects()
    {
        var next = ItemBonus.From(
            SaveData.HasItem("Necklace"), SaveData.HasItem("Sword"), SaveData.HasItem("Ring"), SaveData.HasItem("MagicBook"),
            !characterList[0].IsDead(), !characterList[1].IsDead(), !characterList[2].IsDead(), !characterList[3].IsDead());

        int deltaDefense = next.Defense - bonus.Defense;
        int deltaAttack = next.Attack - bonus.Attack;
        int deltaHealerAttack = next.HealerAttack - bonus.HealerAttack;
        bool hasBuffChanged = deltaDefense != 0 || deltaAttack != 0 || deltaHealerAttack != 0;

        bonus = next;

        if (hasBuffChanged)
        {
            UpdateCharacterBuffs(deltaAttack, deltaDefense, deltaHealerAttack);
            itemEffectsChanged();
        }
    }
    private void UpdateCharacterBuffs(int deltaAttack, int deltaDefense, int deltaHealerAttack)
    {
        characterList[3].buffs.UpdateContents(deltaHealerAttack, 0);
        foreach (var character in characterList)
            character.buffs.UpdateContents(deltaAttack, deltaDefense);
    }

    public int GetItemBonus(Character owner)
    {
        var warrior = characterList[0];
        var healer = characterList[3];

        if (owner == warrior) return bonus.Defense;
        if (owner == healer) return bonus.Attack + bonus.HealerAttack;
        return bonus.Attack;
    }

    private Dictionary<string, Action<object, ITarget>> CreateEffects()
    {
        return new Dictionary<string, Action<object, ITarget>>()
        {
            // Cards
            { "Attack",             (_, target) => Attack((Monster)target) },
            { "AttackAll",          (_, _) => AttackAll() },
            { "Shield",             (from, target) => Shield(from, (Character)target) },
            { "ShieldAll",          (_, _) => ShieldAll() },
            { "Buff",               (from, target) => Buff(from, (Character)target) },
            { "Debuff",             (_, target) => Debuff((Monster)target) },
            { "Skill",              (_, target) => Skill((Monster)target) },
            { "Heal",               (from, target) => Heal(from, (Character)target) },
            { "HealAll",            (_, _) => HealAll() },
            // Monster actions
            { "ActionAttack",       (from, target) => ActionAttack((Monster)from, (Character)target) },
            { "ActionDoubleAttack", (from, target) => ActionDoubleAttack((Monster)from, (Character)target) },
            { "ActionShieldAttack", (from, target) => ActionShieldAttack((Monster)from, (Character)target) },
            { "ActionShield",       (from, target) => ActionShield((Monster)from, target) },
            { "ActionBuffAttack",   (from, target) => ActionBuffAttack((Monster)from, target) },
            { "ActionDodgeAttack",  (from, target) => ActionDodgeAttack((Monster)from, target) },
            { "ActionDebuffAttack", (from, target) => ActionDebuffAttack((Monster)from, (Character)target) },
            { "ActionDebuffDraw1",  (from, target) => ActionDebuffDraw1((Monster)from, target) },
            { "ActionDebuffDraw2",  (from, target) => ActionDebuffDraw2((Monster)from, target) },
            { "ActionFreezeHand1",  (from, target) => ActionFreezeHand1((Monster)from, target) },
            { "ActionFreezeHand2",  (from, target) => ActionFreezeHand2((Monster)from, target) },
            // Potions
            { "BluePotion",         (_, _) => BluePotion() },
            { "GreenPotion",        (from, target) => GreenPotion(from, (Character)target) },
            { "Gem",                (from, target) => Gem(from, (Character)target) },
            { "Mushroom",           (from, target) => Mushroom(from, (Character)target) },
            { "Twig",               (_, target) => Twig((Monster)target) },
        };
    }

    // Card effects
    private void Attack(Monster target)
    {
        var swordsman = characterList[1];
        int attack = swordsman.Attack + bonus.Attack;

        swordsman.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(BattleRules.Damage(attack, target.Defense));
    }
    private void AttackAll()
    {
        var swordsman = characterList[1];
        int attack = swordsman.Attack + bonus.Attack;

        swordsman.AnimateAttack();

        foreach (var monster in monsterList)
        {
            monster.AnimateHit();
            monster.UpdateHealth(BattleRules.Damage(attack, monster.Defense));
        }
    }
    private void Shield(object _, Character target)
    {
        var warrior = characterList[0];
        int defense = warrior.Attack + bonus.Defense;
        
        target.AnimateSpell();
        target.UpdateDefense(defense);
    }
    private void ShieldAll()
    {
        var warrior = characterList[0];
        int defense = warrior.Attack + bonus.Defense;
        
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
        int attack = magician.Attack + bonus.Attack;

        magician.AnimateSpell();
        target.AnimateHit();
        target.UpdateHealth(BattleRules.Damage(attack, target.Defense));
    }
    private void Heal(object _, Character target)
    {
        var healer = characterList[3];
        int attack = healer.Attack + bonus.Attack + bonus.HealerAttack;

        target.AnimateSpell();
        target.UpdateHealth(attack);
    }
    private void HealAll()
    {
        var healer = characterList[3];
        int attack = healer.Attack + bonus.Attack + bonus.HealerAttack;
        
        foreach (var character in characterList)
        {
            character.AnimateSpell();
            character.UpdateHealth(attack);
        }
    }

    // Monster actions
    private void ActionAttack(Monster monster, Character target)
    {
        int defense = target.Defense + bonus.Defense;

        monster.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(BattleRules.Damage(monster.Attack, defense));
    }
    private void ActionDoubleAttack(Monster monster, Character target)
    {
        int defense = target.Defense + bonus.Defense;

        monster.AnimateAttack();
        target.AnimateHit();
        target.UpdateHealth(BattleRules.Damage(monster.Attack * 2, defense));
    }
    private void ActionShieldAttack(Monster monster, Character target)
    {
        int defense = target.Defense + bonus.Defense;

        monster.AnimateAttack();
        monster.UpdateDefense(6);
        target.AnimateHit();
        target.UpdateHealth(BattleRules.Damage(monster.Attack, defense));
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
        int debuff = -3 + bonus.ResistDebuff;

        monster.AnimateSpell();
        target.AnimateHit();
        target.UpdateAttack(debuff);
    }
    private void ActionDebuffDraw1(Monster monster, object _)
    {
        int debuff = 1 - bonus.ResistDebuff;

        monster.AnimateSpell();
        turnManager.ReduceDraw(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionDebuffDraw2(Monster monster, object _)
    {
        int debuff = 2 - bonus.ResistDebuff;

        monster.AnimateSpell();
        turnManager.ReduceDraw(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionFreezeHand1(Monster monster, object _)
    {
        int debuff = 1 - bonus.ResistDebuff;

        monster.AnimateSpell();
        hand.SetFreeze(debuff);
        
        foreach (var character in characterList)
            character.AnimateHit();
    }
    private void ActionFreezeHand2(Monster monster, object _)
    {
        int debuff = 2 - bonus.ResistDebuff;

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
