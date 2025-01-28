using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return DataManager.Instance.Initialize();
        yield return BgmManager.Instance.Initialize();
        SceneManager.LoadSceneAsync(SceneName.Opening.ToSceneString());
    }
}
