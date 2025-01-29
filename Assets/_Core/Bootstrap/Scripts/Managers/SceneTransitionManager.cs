using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BgmType = BgmManager.BgmType;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private Dictionary<SceneName, BgmType> sceneToBgm = new Dictionary<SceneName, BgmType>()
    {
        { SceneName.Opening,            BgmType.Opening },
        { SceneName.CharacterSelect,    BgmType.NonBattle },
        { SceneName.Level,              BgmType.NonBattle },
        { SceneName.Battle,             BgmType.Battle },
        { SceneName.Ending,             BgmType.NonBattle },
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWithCrossfade(SceneName sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }

    private IEnumerator TransitionScene(SceneName sceneName)
    {
        var from = (sceneName == SceneName.Battle) ? BgmType.NonBattle : BgmType.Battle;
        var to = sceneToBgm[sceneName];
        BgmManager.Instance.CrossFade(from, to);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToSceneString());
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);
        asyncLoad.allowSceneActivation = true;
    }
}
