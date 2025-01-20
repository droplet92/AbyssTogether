using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("LevelScene");
    }
}
