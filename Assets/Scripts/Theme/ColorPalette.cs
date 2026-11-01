using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "Abyss Together/Color Palette")]
public class ColorPalette : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public ColorToken token;
        public Color color;
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    private Dictionary<ColorToken, Color> lookup;

    public Color Get(ColorToken token)
    {
        if (lookup == null) BuildLookup();
        if (lookup.TryGetValue(token, out var color)) return color;

        Debug.LogError($"{name} has no color for {token}");
        return Color.magenta;
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<ColorToken, Color>();
        foreach (var entry in entries)
            lookup[entry.token] = entry.color;
    }

    void OnValidate()
    {
        var present = new HashSet<ColorToken>();
        foreach (var entry in entries) present.Add(entry.token);
        foreach (ColorToken token in Enum.GetValues(typeof(ColorToken)))
        {
            if (token != ColorToken.None && !present.Contains(token))
                entries.Add(new Entry { token = token, color = Color.magenta });
        }
        lookup = null;
    }
}
