using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text escapeTime;
    [SerializeField] private List<Sprite> backgroundList;
    [SerializeField] private Button openingButton;

    void Awake()
    {
        string startTime = PlayerPrefs.GetString("StartTime");
        var diff = DateTime.Now - DateTime.Parse(startTime);
        escapeTime.text = $"{diff.Minutes}:{diff.Seconds}";

        int playerCharacter = PlayerPrefs.GetInt("PlayerCharacter") - 1;
        background.sprite = backgroundList[playerCharacter];

        openingButton.onClick.AddListener(ToOpening);
    }

    private void ToOpening()
    {
        BgmManager.Instance.StopAll();
        SceneManager.LoadScene(SceneName.Opening.ToSceneString());
    }
}
