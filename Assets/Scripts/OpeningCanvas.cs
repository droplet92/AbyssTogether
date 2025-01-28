using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BgmType = BgmManager.BgmType;

public class OpeningCanvas : MonoBehaviour
{
    [SerializeField] private CanvasRenderer settingsPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    
    void Awake()
    {
        Debug.Assert(settingsPanel != null, "SerializeField is empty: Settings Panel");
        Debug.Assert(startButton != null, "SerializeField is empty: Start Button");
        Debug.Assert(settingsButton != null, "SerializeField is empty: Settings Button");
        Debug.Assert(exitButton != null, "SerializeField is empty: Exit Button");
        
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ActivateSettingsPanel);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        BgmManager.Instance.CrossFade(BgmType.Opening, BgmType.NonBattle);
        SceneTransitionManager.Instance.LoadSceneWithCrossfade(SceneName.CharacterSelect);
    }
    private void ActivateSettingsPanel()
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
