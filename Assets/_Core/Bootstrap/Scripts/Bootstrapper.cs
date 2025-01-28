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
        yield return GameManager.Instance.Initialize();
        yield return BgmManager.Instance.Initialize();
        SceneManager.LoadSceneAsync("OpeningScene");
    }
}
