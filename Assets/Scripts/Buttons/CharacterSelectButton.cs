using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : BaseButton
{
    [SerializeField] private int playerCharacter;

    public override void OnClick()
    {
        base.OnClick();
        PlayerPrefs.SetInt("PlayerCharacter", playerCharacter);
        PlayerPrefs.SetString("StartTime", DateTime.Now.ToLongTimeString());
        SceneManager.LoadScene("LevelScene");
    }
}
