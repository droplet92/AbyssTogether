using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer logoPlayer;
    [SerializeField] private VideoPlayer introPlayer;

    public delegate void AllVideoEndHandler();
    public event AllVideoEndHandler OnAllVideoEnd;
    
    void Start()
    {
    #if !UNITY_WEBGL
        logoPlayer.Play();
    #else
        OnAllVideoEnd?.Invoke();
    #endif
    }

#if !UNITY_WEBGL
    void Awake()
    {
        logoPlayer.loopPointReached += PlayIntro;
        introPlayer.loopPointReached += PlayGame;
    }
    void Update()
    {
        if (introPlayer.isPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
            PlayGame(introPlayer);
    }

    private void PlayIntro(VideoPlayer vp)
    {
        logoPlayer.gameObject.SetActive(false);
        introPlayer.Play();
    }
    private void PlayGame(VideoPlayer vp)
    {
        introPlayer.gameObject.SetActive(false);

        OnAllVideoEnd?.Invoke();
    }
#endif
}