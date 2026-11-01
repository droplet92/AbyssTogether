using UnityEngine;

public class OpeningSceneManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private OpeningCanvas openingCanvas;
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private VideoPlayerManager videoPlayerManager;

    void Awake()
    {
        openingCanvas.Initialize();
        videoPlayerManager.Initialize();
        videoPlayerManager.OnAllVideoEnd += PlayGame;
    }
    void Start()
    {
        settingsPanel.gameObject.SetActive(false);
        videoPlayerManager.Begin();
    }

    private void PlayGame()
    {
        canvas.gameObject.SetActive(true);
        BgmManager.Instance.PlayOpeningBGM();
    }
}
