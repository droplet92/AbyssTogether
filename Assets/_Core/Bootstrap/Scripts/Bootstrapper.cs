using System.Collections;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private BgmManager bgmManager;
    [SerializeField] private SfxManager sfxManager;
    [SerializeField] private SceneTransitionManager sceneTransitionManager;

    void Awake()
    {
        UseInvariantNumberFormat();
        Palette.ApplyTextStyles();
        dataManager.Register();
        bgmManager.Register();
        sfxManager.Register();
        sceneTransitionManager.Register();
        StartCoroutine(Initialize());
    }

    private static void UseInvariantNumberFormat()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
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
