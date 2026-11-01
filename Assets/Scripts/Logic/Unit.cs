using System;

public class Unit
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public Unit(int maxHealth, int currentHealth, int attack, int defense)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Attack = attack;
        Defense = defense;
    }

    // Each change returns the amount actually applied, or null when the unit is dead.
    public int? ChangeHealth(int delta)
    {
        if (IsDead) return null;

        int applied = Math.Min(delta, MaxHealth);
        CurrentHealth = Math.Min(CurrentHealth + applied, MaxHealth);
        return applied;
    }
    public int? ChangeAttack(int delta)
    {
        if (IsDead) return null;

        int before = Attack;
        Attack = Math.Max(Attack + delta, 0);
        return Attack - before;
    }
    public int? ChangeDefense(int delta)
    {
        if (IsDead) return null;

        int before = Defense;
        Defense = Math.Max(Defense + delta, 0);
        return Defense - before;
    }
}
