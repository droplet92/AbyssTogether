using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CampExitButton : MonoBehaviour
{
    [SerializeField] private Image fadeOut;

    virtual public void OnClick()
    {
        int level = PlayerPrefs.GetInt("level");

        PlayerPrefs.SetInt("level", level + 1);
        fadeOut.gameObject.SetActive(true);
        fadeOut.DOFade(1f, 1f)
            .OnComplete(() => SceneManager.LoadScene(SceneName.Level.ToSceneString()));
    }
}
