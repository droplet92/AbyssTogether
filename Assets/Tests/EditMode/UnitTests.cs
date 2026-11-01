using NUnit.Framework;

public class UnitTests
{
    [Test]
    public void Healing_StopsAtMaxHealth()
    {
        var unit = new Unit(maxHealth: 30, currentHealth: 28, attack: 3, defense: 0);

        int? applied = unit.ChangeHealth(5);

        Assert.AreEqual(30, unit.CurrentHealth);
        Assert.AreEqual(5, applied);
    }

    [Test]
    public void Damage_CanDropHealthBelowZero()
    {
        var unit = new Unit(30, 4, 3, 0);

        unit.ChangeHealth(-6);

        Assert.AreEqual(-2, unit.CurrentHealth);
        Assert.IsTrue(unit.IsDead);
    }

    [Test]
    public void DeadUnit_IgnoresAllChanges()
    {
        var unit = new Unit(30, 0, 3, 2);

        Assert.IsNull(unit.ChangeHealth(10));
        Assert.IsNull(unit.ChangeAttack(1));
        Assert.IsNull(unit.ChangeDefense(1));
        Assert.AreEqual(0, unit.CurrentHealth);
        Assert.AreEqual(3, unit.Attack);
        Assert.AreEqual(2, unit.Defense);
    }

    [Test]
    public void Attack_DoesNotGoBelowZero_AndReportsActualChange()
    {
        var unit = new Unit(30, 30, attack: 2, defense: 0);

        int? applied = unit.ChangeAttack(-3);

        Assert.AreEqual(0, unit.Attack);
        Assert.AreEqual(-2, applied);
    }

    [Test]
    public void Defense_DoesNotGoBelowZero_AndReportsActualChange()
    {
        var unit = new Unit(30, 30, attack: 0, defense: 1);

        int? applied = unit.ChangeDefense(-5);

        Assert.AreEqual(0, unit.Defense);
        Assert.AreEqual(-1, applied);
    }
}
