using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    public const int MaxHealth = 30;
    public const int PotionSlotCount = 3;

    public static readonly string[] PartyMembers = { "Warrior", "Swordsman", "Magician", "Healer" };
    public static readonly string[] Items = { "Necklace", "Sword", "Ring", "MagicBook" };

    private static readonly string[] DefaultDeck =
    {
        "Attack", "Attack", "AttackAll",
        "Shield", "Shield", "ShieldAll",
        "Heal", "Heal", "HealAll",
        "Buff", "Debuff", "Skill"
    };

    public static int Level
    {
        get => PlayerPrefs.GetInt("level");
        set => PlayerPrefs.SetInt("level", value);
    }
    public static int PlayerCharacter
    {
        get => PlayerPrefs.GetInt("PlayerCharacter");
        set => PlayerPrefs.SetInt("PlayerCharacter", value);
    }
    public static string StartTime
    {
        get => PlayerPrefs.GetString("StartTime");
        set => PlayerPrefs.SetString("StartTime", value);
    }

    public static int GetHealth(string member, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt($"Hp{member}", defaultValue);
    }
    public static void SetHealth(string member, int value)
    {
        PlayerPrefs.SetInt($"Hp{member}", value);
    }

    public static bool HasItem(string item)
    {
        return PlayerPrefs.GetInt($"Item{item}") != 0;
    }
    public static void SetItem(string item, bool owned)
    {
        PlayerPrefs.SetInt($"Item{item}", owned ? 1 : 0);
    }

    public static string GetPotion(int slot)
    {
        return PlayerPrefs.GetString($"Potion{slot}");
    }
    public static void SetPotion(int slot, string potion)
    {
        PlayerPrefs.SetString($"Potion{slot}", potion);
    }

    public static List<string> LoadDeck()
    {
        var data = PlayerPrefs.GetString("Deck");
        if (data.Length == 0)
        {
            var deck = new List<string>(DefaultDeck);
            SaveDeck(deck);
            return deck;
        }
        return JsonUtility.FromJson<DeckDataJson>(data).cards;
    }
    public static void SaveDeck(List<string> cards)
    {
        PlayerPrefs.SetString("Deck", JsonUtility.ToJson(new DeckDataJson { cards = cards }));
    }

    public static void StartNewGame()
    {
        Level = 1;
        PlayerPrefs.SetString("Deck", null);

        foreach (var member in PartyMembers)
            SetHealth(member, MaxHealth);

        foreach (var item in Items)
            SetItem(item, false);

        for (int slot = 1; slot <= PotionSlotCount; slot++)
            SetPotion(slot, null);
    }
}
