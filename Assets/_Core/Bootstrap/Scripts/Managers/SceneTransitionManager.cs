using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BgmType = BgmManager.BgmType;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private Dictionary<string, BgmType> sceneToBgm = new Dictionary<string, BgmType>()
    {
        { "OpeningScene", BgmType.Opening },
        { "BattleScene", BgmType.Battle },
        { "LevelScene", BgmType.NonBattle },
        { "CharacterSelectScene", BgmType.NonBattle },
        { "EndingScene", BgmType.NonBattle },
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

    public void LoadSceneWithCrossfade(string sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }

    private IEnumerator TransitionScene(string sceneName)
    {
        var from = (sceneName == "BattleScene") ? BgmType.NonBattle : BgmType.Battle;
        var to = sceneToBgm[sceneName];
        BgmManager.Instance.CrossFade(from, to);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);
        asyncLoad.allowSceneActivation = true;
    }
}
