using System.Collections.Generic;

public enum SceneName
{
    Opening,
    CharacterSelect,
    Level,
    Battle,
    Camp,
    Ending
}

public static class Extenders
{
    private static Dictionary<SceneName, string> toString = new Dictionary<SceneName, string>()
    {
        { SceneName.Opening,            "1_OpeningScene" },
        { SceneName.CharacterSelect,    "2_CharacterSelectScene" },
        { SceneName.Level,              "3_LevelScene" },
        { SceneName.Battle,             "4_BattleScene" },
        { SceneName.Camp,               "5_CampScene" },
        { SceneName.Ending,             "6_EndingScene" },
    };

    public static string ToSceneString(this SceneName sceneName)
    {
        return toString[sceneName];
    }
}
