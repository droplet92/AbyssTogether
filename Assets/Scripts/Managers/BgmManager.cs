using System.Collections;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] private AudioSource openingBGM;
    [SerializeField] private AudioSource combatBGM;
    [SerializeField] private AudioSource nonCombatBGM;

    public static BgmManager Instance { get; private set; }

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

    public void PlayOpeningBGM()
    {
        openingBGM.Play();
    }
    public void StopOpeningBGM()
    {
        StartCoroutine(CrossFade(nonCombatBGM, openingBGM));
    }
    public void SwitchToOpeningBGM()
    {
        StartCoroutine(CrossFade(openingBGM, combatBGM));
    }
    public void SwitchToCombatBGM()
    {
        StartCoroutine(CrossFade(combatBGM, nonCombatBGM));
    }
    public void SwitchToNonCombatBGM()
    {
        StartCoroutine(CrossFade(nonCombatBGM, combatBGM));
    }

    private IEnumerator CrossFade(AudioSource fadeInSource, AudioSource fadeOutSource)
    {
        fadeInSource.Play();

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
        fadeOutSource.Stop();
    }
}
