using TMPro;
using UnityEngine;

public class CampManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    void Start()
    {
        int level = PlayerPrefs.GetInt("level");
        levelText.text = level.ToString();
    }
}
