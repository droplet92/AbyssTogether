using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    
    void Awake()
    {
        Debug.Assert(buttons.Count == 4, "SerializeField size changed: Buttons");

        for (int i = 0; i < buttons.Count; ++i)
        {
            int playerCharacter = i + 1;
            buttons[i].onClick.AddListener(() => SetPlayerCharacter(playerCharacter));
        }
    }

    private void SetPlayerCharacter(int playerCharacter)
    {
        SaveData.PlayerCharacter = playerCharacter;
        SaveData.StartTime = DateTime.UtcNow.Ticks.ToString();
        SceneManager.LoadScene(SceneName.Level.ToSceneString());
    }
}
