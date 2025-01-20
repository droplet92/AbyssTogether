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

    public void LoadSceneWithCrossfade(string sceneName, bool toCombat)
    {
        StartCoroutine(TransitionScene(sceneName, toCombat));
    }
    public void LoadOpeningSceneWithCrossfade()
    {
        StartCoroutine(TransitionOpeningScene());
    }

    private IEnumerator TransitionScene(string sceneName, bool toCombat)
    {
        if (toCombat)
            BgmManager.Instance.SwitchToCombatBGM();
        else
            BgmManager.Instance.SwitchToNonCombatBGM();

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
