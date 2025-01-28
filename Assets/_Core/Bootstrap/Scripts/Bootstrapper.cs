using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrapper : AutoFieldValidator
{
    [SerializeField] private Image logo;

    void Awake()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return DataManager.Instance.Initialize();
        yield return BgmManager.Instance.Initialize();
        yield return ShowLogo();
        SceneManager.LoadSceneAsync(SceneName.Opening.ToSceneString());
    }
    private IEnumerator ShowLogo()
    {
        logo.DOFade(0f, 2f).SetEase(Ease.InOutQuart);
        yield return new WaitForSeconds(2f);
    }
}
