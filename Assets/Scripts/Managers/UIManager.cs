using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasRenderer settingsPanel;
#if UNITY_WEBGL == false
    [SerializeField] private VideoController videoController;
#endif

    void Start()
    {
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
            settingsPanel.gameObject.SetActive(false);

        #if UNITY_WEBGL
            ActivateStartCanvas();
        #else
            videoController.OnAllVideoEnd += ActivateStartCanvas;
        #endif
        }
    }
    private void ActivateStartCanvas()
    {
        BgmManager.Instance.PlayOpeningBGM();
        canvas.gameObject.SetActive(true);
    }
}
