using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    private const string IntroFileName = "intro.mp4";

    [SerializeField] private VideoPlayer introPlayer;

    public delegate void AllVideoEndHandler();
    public event AllVideoEndHandler OnAllVideoEnd;
    
    void Start()
    {
    #if !UNITY_WEBGL
        introPlayer.url = Path.Combine(Application.streamingAssetsPath, IntroFileName);
        introPlayer.Prepare();
    #else
        OnAllVideoEnd?.Invoke();
    #endif
    }

#if !UNITY_WEBGL
    void Awake()
    {
        introPlayer.prepareCompleted += vp => vp.Play();
        introPlayer.loopPointReached += PlayGame;
    }
    void Update()
    {
        if (!introPlayer.isPlaying) return;
        if (!Input.anyKeyDown) return;  // also true for mouse buttons
        PlayGame(introPlayer);
    }

    private void PlayGame(VideoPlayer vp)
    {
        introPlayer.gameObject.SetActive(false);
        OnAllVideoEnd?.Invoke();
    }
#endif
}