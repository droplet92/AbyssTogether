using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSetting : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    public enum Language
    {
        Korean,
        English,
        Japanese
    }

    private void Start()
    {
        var defaultLanguage = PlayerPrefs.GetInt("Language", (int)Language.English);
        
        SetLanguage(defaultLanguage);
    }
    public void SetLanguage(int value)
    {
        var selection = (Language)value;
        dropdown.value = value;

        PlayerPrefs.SetInt("Language", value);

        switch (selection)
        {
            case Language.Korean:
                LoadLocale("ko-KR");
                break;
            case Language.English:
                LoadLocale("en-US");
                break;
            case Language.Japanese:
                LoadLocale("ja-JP");
                break;
            default:
                Debug.LogError("미구현 모드입니다.");
                break;
        }
    }
    private void LoadLocale(string languageIdentifier)
    {
        var localeCode = new LocaleIdentifier(languageIdentifier);

        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
            LocaleIdentifier anIdentifier = aLocale.Identifier;

            if(anIdentifier == localeCode)
                LocalizationSettings.SelectedLocale = aLocale;
        }
    }
}