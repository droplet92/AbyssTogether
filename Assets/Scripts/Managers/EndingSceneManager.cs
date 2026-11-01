using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text escapeTime;
    [SerializeField] private List<Sprite> backgroundList;
    [SerializeField] private Button openingButton;

    void Awake()
    {
        string startTime = SaveData.StartTime;
        var diff = DateTime.UtcNow - new DateTime(long.Parse(startTime), DateTimeKind.Utc);
        escapeTime.text = $"{(int)diff.TotalMinutes}:{diff.Seconds:D2}";

        int playerCharacter = SaveData.PlayerCharacter - 1;
        background.sprite = backgroundList[playerCharacter];

        openingButton.onClick.AddListener(ToOpening);
    }

    private void ToOpening()
    {
        BgmManager.Instance.StopAll();
        SceneManager.LoadScene(SceneName.Opening.ToSceneString());
    }
}
