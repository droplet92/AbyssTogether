using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text labelWin;
    [SerializeField] private TMP_Text labelLose;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button potionButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button okButton;

    public void ShowWin()
    {
        int level = PlayerPrefs.GetInt("level");

        if (level == 4 || level == 8 || level == 12)
            itemButton.gameObject.SetActive(true);
        else
            itemButton.gameObject.SetActive(false);

        labelLose.gameObject.SetActive(false);
        okButton.onClick.AddListener(Win);
        canvasGroup.DOFade(1f, 0.5f);
    }

    public void ShowDefeat()
    {
        labelWin.gameObject.SetActive(false);
        potionButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
        okButton.onClick.AddListener(Defeat);
        canvasGroup.DOFade(1f, 0.5f);
    }

    private void Win()
    {
        int level = PlayerPrefs.GetInt("level");

        PlayerPrefs.SetInt("level", level + 1);

        if (level == 17)
            SceneManager.LoadScene("OpeningScene");
        else
            SceneManager.LoadScene("LevelScene");
    }

    private void Defeat()
    {
        SceneManager.LoadScene("OpeningScene");
    }
}
