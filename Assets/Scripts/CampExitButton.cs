using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CampExitButton : MonoBehaviour
{
    [SerializeField] private Image image;

    public virtual void OnClick()
    {
        int level = PlayerPrefs.GetInt("level");

        PlayerPrefs.SetInt("level", level + 1);

        image.gameObject.SetActive(true);
        image.DOFade(1f, 1.5f)
            .OnComplete(() => SceneManager.LoadScene("LevelScene"));
    }
}
