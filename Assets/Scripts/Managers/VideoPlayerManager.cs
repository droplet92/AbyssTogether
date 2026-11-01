using System;
using System.IO;
using System.Runtime.InteropServices;
using AOT;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    private const string IntroFileName = "intro.mp4";

    [SerializeField] private VideoPlayer introPlayer;
    [SerializeField] private TMP_Text startPrompt;

    public delegate void AllVideoEndHandler();
    public event AllVideoEndHandler OnAllVideoEnd;

    private bool isStarted = false;
    private int startFrame;

#if UNITY_WEBGL && !UNITY_EDITOR
    private static VideoPlayerManager instance;

    [DllImport("__Internal")]
    private static extern void RegisterFirstInput(Action callback);

    [MonoPInvokeCallback(typeof(Action))]
    private static void OnFirstInput()
    {
        instance.StartIntro();
    }
#endif

    void Awake()
    {
        introPlayer.prepareCompleted += vp => TryPlay();
        introPlayer.loopPointReached += PlayGame;
        introPlayer.errorReceived += (vp, message) => PlayGame(vp);
    }
    void Start()
    {
        introPlayer.url = Path.Combine(Application.streamingAssetsPath, IntroFileName);
        introPlayer.Prepare();

    #if UNITY_WEBGL && !UNITY_EDITOR
        instance = this;
        startPrompt.gameObject.SetActive(true);
        startPrompt.DOFade(0.2f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        RegisterFirstInput(OnFirstInput);
    #else
        StartIntro();
    #endif
    }
    void Update()
    {
        if (!introPlayer.isPlaying) return;
        if (Time.frameCount <= startFrame + 1) return;
        if (!Input.anyKeyDown) return;  // also true for mouse buttons
        PlayGame(introPlayer);
    }

    private void StartIntro()
    {
        isStarted = true;
        startFrame = Time.frameCount;
        HideStartPrompt();
        TryPlay();
    }
    private void TryPlay()
    {
        if (isStarted && introPlayer.isPrepared)
            introPlayer.Play();
    }
    private void HideStartPrompt()
    {
        startPrompt.DOKill();
        startPrompt.gameObject.SetActive(false);
    }
    private void PlayGame(VideoPlayer vp)
    {
        HideStartPrompt();
        introPlayer.gameObject.SetActive(false);
        OnAllVideoEnd?.Invoke();
    }
}