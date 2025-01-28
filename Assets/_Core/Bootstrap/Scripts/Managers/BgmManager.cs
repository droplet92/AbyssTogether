using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] private AudioSource openingBGM;
    [SerializeField] private AudioSource battleBGM;
    [SerializeField] private AudioSource nonBattleBGM;

    public enum BgmType { Opening, Battle, NonBattle }
    public static BgmManager Instance { get; private set; }
    
    private Dictionary<BgmType, AudioSource> toAudioSource;

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
        toAudioSource = new Dictionary<BgmType, AudioSource>()
        {
            { BgmType.Opening, openingBGM },
            { BgmType.Battle, battleBGM },
            { BgmType.NonBattle, nonBattleBGM },
        };
        battleBGM.Play();
        nonBattleBGM.Play();
        yield break;
    }

    public void PlayOpeningBGM()
    {
        openingBGM.Play();
    }
    public void CrossFade(BgmType from, BgmType to)
    {
        StartCoroutine(CrossFade(toAudioSource[from], toAudioSource[to]));
    }

    private IEnumerator CrossFade(AudioSource fadeOutSource, AudioSource fadeInSource)
    {
        const float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            fadeInSource.volume = Mathf.Lerp(0f, duration, t);
            fadeOutSource.volume = Mathf.Lerp(duration, 0f, t);
            yield return null;
        }
        fadeInSource.volume = 1f;
        fadeOutSource.volume = 0f;
    }
}
