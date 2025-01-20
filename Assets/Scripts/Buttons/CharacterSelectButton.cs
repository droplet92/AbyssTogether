using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : BaseButton
{
    [SerializeField] private int playerCharacter;

    public override void OnClick()
    {
        base.OnClick();
        PlayerPrefs.SetInt("PlayerCharacter", playerCharacter);
        SceneManager.LoadScene("LevelScene");
    }
}
