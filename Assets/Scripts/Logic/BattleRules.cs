using System;

public static class BattleRules
{
    public static int Damage(int attack, int defense)
    {
        return Math.Min(defense - attack, 0);
    }
}
