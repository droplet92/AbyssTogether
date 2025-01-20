using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer logoPlayer;
    [SerializeField] private VideoPlayer introPlayer;

    public delegate void AllVideoEndHandler();
    public event AllVideoEndHandler OnAllVideoEnd;
    
    void Start()
    {
        Debug.Log("Video starts.");
        
        logoPlayer.loopPointReached += OnLogoEnd;
        introPlayer.loopPointReached += OnIntroEnd;
    }
    void Update()
    {
        if (introPlayer.isPlaying && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
            OnIntroEnd(introPlayer);
    }

    private void OnLogoEnd(VideoPlayer vp)
    {
        logoPlayer.gameObject.SetActive(false);
        introPlayer.Play();
    }
    private void OnIntroEnd(VideoPlayer vp)
    {
        introPlayer.gameObject.SetActive(false);

        OnAllVideoEnd?.Invoke();
    }
}
