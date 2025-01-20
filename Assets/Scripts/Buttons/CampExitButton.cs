using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CampExitButton : BaseButton
{
    [SerializeField] private Image fadeOut;

    public override void OnClick()
    {
        base.OnClick();

        int level = PlayerPrefs.GetInt("level");

        PlayerPrefs.SetInt("level", level + 1);
        fadeOut.gameObject.SetActive(true);
        fadeOut.DOFade(1f, 1f)
            .OnComplete(() => SceneManager.LoadScene("LevelScene"));
    }
    protected void BaseOnClick()
    {
        base.OnClick();
    }
}
