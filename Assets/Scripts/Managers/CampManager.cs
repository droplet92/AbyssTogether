using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampManager : AutoFieldValidator
{
    [SerializeField] private Image fadeOut;
    [SerializeField] private TMP_Text levelText;

    private int level = 0;

    void Awake()
    {
        level = PlayerPrefs.GetInt("level");
        levelText.text = level.ToString();
    }

    public void ExitCamp()
    {
        PlayerPrefs.SetInt("level", level + 1);

        fadeOut.gameObject.SetActive(true);
        fadeOut.DOFade(1f, 1f)
            .OnComplete(() => SceneManager.LoadScene(SceneName.Level.ToSceneString()));
    }
}
