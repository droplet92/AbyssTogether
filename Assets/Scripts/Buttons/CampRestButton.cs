using UnityEngine;

public class CampRestButton : CampExitButton
{
    public override void OnClick()
    {
        int healerHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpHealer") * 1.2f));
        int magicianHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpMagician") * 1.2f));
        int swordsmanHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpSwordsman") * 1.2f));
        int warriorHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpWarrior") * 1.2f));
        
        PlayerPrefs.SetInt($"HpHealer", healerHp);
        PlayerPrefs.SetInt($"HpMagician", magicianHp);
        PlayerPrefs.SetInt($"HpSwordsman", swordsmanHp);
        PlayerPrefs.SetInt($"HpWarrior", warriorHp);

        base.OnClick();
    }
}
