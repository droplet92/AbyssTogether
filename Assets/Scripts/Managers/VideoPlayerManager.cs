using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer introPlayer;

    public delegate void AllVideoEndHandler();
    public event AllVideoEndHandler OnAllVideoEnd;
    
    void Start()
    {
    #if !UNITY_WEBGL
        introPlayer.Play();
    #else
        OnAllVideoEnd?.Invoke();
    #endif
    }

#if !UNITY_WEBGL
    void Awake()
    {
        introPlayer.loopPointReached += PlayGame;
    }
    void Update()
    {
        if (!introPlayer.isPlaying) return;
        if (!Input.anyKeyDown) return;
        if (!Input.GetMouseButtonUp(0)) return;
        PlayGame(introPlayer);
    }

    private void PlayGame(VideoPlayer vp)
    {
        introPlayer.gameObject.SetActive(false);
        OnAllVideoEnd?.Invoke();
    }
#endif
}