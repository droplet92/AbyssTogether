using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : AutoFieldValidator
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
        PlayerPrefs.SetInt("PlayerCharacter", playerCharacter);
        PlayerPrefs.SetString("StartTime", DateTime.Now.ToLongTimeString());
        SceneManager.LoadScene(SceneName.Level.ToSceneString());
    }
}
