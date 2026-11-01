using NUnit.Framework;

public class BattleRulesTests
{
    [TestCase(5, 0, -5)]
    [TestCase(5, 3, -2)]
    [TestCase(5, 5, 0)]
    [TestCase(5, 9, 0)]
    public void Damage_IsAttackMinusDefense_AndNeverHeals(int attack, int defense, int expected)
    {
        Assert.AreEqual(expected, BattleRules.Damage(attack, defense));
    }

    [Test]
    public void ItemBonus_RequiresOwnerToBeAlive()
    {
        var bonus = ItemBonus.From(necklace: true, sword: true, ring: true, magicBook: true,
                                   warriorAlive: true, swordsmanAlive: false, magicianAlive: true, healerAlive: false);

        Assert.AreEqual(1, bonus.Defense);
        Assert.AreEqual(0, bonus.Attack);
        Assert.AreEqual(1, bonus.ResistDebuff);
        Assert.AreEqual(0, bonus.HealerAttack);
    }

    [Test]
    public void ItemBonus_IsZeroWithoutItems()
    {
        var bonus = ItemBonus.From(false, false, false, false, true, true, true, true);

        Assert.AreEqual(0, bonus.Attack + bonus.Defense + bonus.ResistDebuff + bonus.HealerAttack);
    }
}
