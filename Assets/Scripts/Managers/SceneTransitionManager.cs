using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void LoadSceneWithCrossfade(string sceneName, bool toBattle)
    {
        StartCoroutine(TransitionScene(sceneName, toBattle));
    }
    public void LoadOpeningSceneWithCrossfade()
    {
        StartCoroutine(TransitionOpeningScene());
    }

    private IEnumerator TransitionScene(string sceneName, bool toBattle)
    {
        if (toBattle)
            BgmManager.Instance.SwitchToBattleBGM();
        else
            BgmManager.Instance.SwitchToNonBattleBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;
    }
    private IEnumerator TransitionOpeningScene()
    {
        BgmManager.Instance.SwitchToOpeningBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("OpeningScene");
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;
    }
}
