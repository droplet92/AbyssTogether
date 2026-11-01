public readonly struct ItemBonus
{
    public int Attack { get; }
    public int Defense { get; }
    public int ResistDebuff { get; }
    public int HealerAttack { get; }

    public ItemBonus(int attack, int defense, int resistDebuff, int healerAttack)
    {
        Attack = attack;
        Defense = defense;
        ResistDebuff = resistDebuff;
        HealerAttack = healerAttack;
    }

    // Each item works only while the party member who carries it is alive.
    public static ItemBonus From(bool necklace, bool sword, bool ring, bool magicBook,
                                 bool warriorAlive, bool swordsmanAlive, bool magicianAlive, bool healerAlive)
    {
        return new ItemBonus(
            attack: sword && swordsmanAlive ? 1 : 0,
            defense: necklace && warriorAlive ? 1 : 0,
            resistDebuff: ring && magicianAlive ? 1 : 0,
            healerAttack: magicBook && healerAlive ? 1 : 0);
    }
}
