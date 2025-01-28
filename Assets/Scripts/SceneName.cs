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
    public static string ToSceneString(this SceneName sceneName)
    {
        switch (sceneName)
        {
        case SceneName.Opening:         return "1_OpeningScene";
        case SceneName.CharacterSelect: return "2_CharacterSelectScene";
        case SceneName.Level:           return "3_LevelScene";
        case SceneName.Battle:          return "4_BattleScene";
        case SceneName.Camp:            return "5_CampScene";
        case SceneName.Ending:          return "6_EndingScene";
        }
        return null;    // never reached
    }
}
