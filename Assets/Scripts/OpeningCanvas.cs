using UnityEngine;
using UnityEngine.UI;
using BgmType = BgmManager.BgmType;

public class OpeningCanvas : AutoFieldValidator
{
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    
    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        BgmManager.Instance.CrossFade(BgmType.Opening, BgmType.NonBattle);
        SceneTransitionManager.Instance.LoadSceneWithCrossfade(SceneName.CharacterSelect);
    }
    private void ToggleSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
    }
    private void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_WEBGL
    #else
        Application.Quit();
    #endif
    }
}
