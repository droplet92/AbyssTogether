using System.Reflection;
using TMPro;
using UnityEngine;

public static class Palette
{
    private static ColorPalette current;

    public static ColorPalette Current
    {
        get
        {
            if (current == null) current = Resources.Load<ThemeSettings>("Theme").palette;
            return current;
        }
    }

    public static Color Get(ColorToken token) => Current.Get(token);

    public static Color Get(ColorToken token, float alpha)
    {
        var color = Current.Get(token);
        color.a = alpha;
        return color;
    }

    public static readonly (string style, ColorToken token)[] TextStyles =
    {
        ("Accent", ColorToken.Accent),
        ("Positive", ColorToken.StatusPositive),
        ("ValueNeutral", ColorToken.TextValueNeutral),
    };

    private static readonly FieldInfo OpeningDefinition = typeof(TMP_Style).GetField("m_OpeningDefinition", BindingFlags.Instance | BindingFlags.NonPublic);

    public static void ApplyTextStyles()
    {
        var sheet = TMP_Settings.defaultStyleSheet;
        foreach (var (styleName, token) in TextStyles)
        {
            var style = sheet.GetStyle(styleName);
            if (style == null)
            {
                Debug.LogError($"Style sheet has no style {styleName}");
                continue;
            }
            OpeningDefinition.SetValue(style, $"<color=#{ColorUtility.ToHtmlStringRGB(Get(token))}>");
            style.RefreshStyle();
        }
    }
}
