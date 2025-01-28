using System.Collections;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] private AudioSource openingBGM;
    [SerializeField] private AudioSource battleBGM;
    [SerializeField] private AudioSource nonBattleBGM;

    public static BgmManager Instance { get; private set; }

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

    public IEnumerator Initialize()
    {
        battleBGM.Play();
        nonBattleBGM.Play();
        yield break;
    }

    public void PlayOpeningBGM()
    {
        openingBGM.Play();
    }
    public void StopOpeningBGM()
    {
        StartCoroutine(CrossFade(nonBattleBGM, openingBGM));
    }
    public void SwitchToOpeningBGM()
    {
        StartCoroutine(CrossFade(openingBGM, battleBGM));
    }
    public void SwitchToBattleBGM()
    {
        StartCoroutine(CrossFade(battleBGM, nonBattleBGM));
    }
    public void SwitchToNonBattleBGM()
    {
        StartCoroutine(CrossFade(nonBattleBGM, battleBGM));
    }

    private IEnumerator CrossFade(AudioSource fadeInSource, AudioSource fadeOutSource)
    {
        const float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            fadeInSource.volume = Mathf.Lerp(0f, 1f, t);
            fadeOutSource.volume = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }
        fadeInSource.volume = 1f;
        fadeOutSource.volume = 0f;
    }
}
