using UnityEngine;

public class OpeningSceneManager : AutoFieldValidator
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private VideoPlayerManager videoPlayerManager;

    void Awake()
    {
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
