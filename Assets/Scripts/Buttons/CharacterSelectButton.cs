using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene("LevelScene");
    }
}
