using UnityEngine;

public class OpeningSceneManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private VideoPlayerManager videoPlayerManager;

    void Awake()
    {
        Debug.Assert(canvas != null, "SerializeField is empty: Canvas");
        Debug.Assert(settingsPanel != null, "SerializeField is empty: Settings Panel");
        Debug.Assert(videoPlayerManager != null, "SerializeField is empty: Video Player Manager");
    
        videoPlayerManager.OnAllVideoEnd += PlayGame;
    }
    void Start()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    private void PlayGame()
    {
        canvas.gameObject.SetActive(true);
        BgmManager.Instance.PlayOpeningBGM();
    }
}
