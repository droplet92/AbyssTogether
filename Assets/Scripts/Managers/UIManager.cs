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
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("HpHealer", 30);
        PlayerPrefs.SetInt("HpMagician", 30);
        PlayerPrefs.SetInt("HpSwordsman", 30);
        PlayerPrefs.SetInt("HpWarrior", 30);
        PlayerPrefs.SetInt("ItemMagicBook", 0);
        PlayerPrefs.SetInt("ItemRing", 0);
        PlayerPrefs.SetInt("ItemSword", 0);
        PlayerPrefs.SetInt("ItemNecklace", 0);
        PlayerPrefs.SetString("Potion1", null);
        PlayerPrefs.SetString("Potion2", null);
        PlayerPrefs.SetString("Potion3", null);
        PlayerPrefs.SetString("Deck", null);
    }
    private void ActivateStartCanvas()
    {
        BgmManager.Instance.PlayOpeningBGM();
        canvas.gameObject.SetActive(true);
    }
}
