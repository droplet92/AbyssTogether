using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private TMP_Text levelText;

    void Awake()
    {
        int level = PlayerPrefs.GetInt("level");
        levelText.text = level.ToString();

        for (int i = 0; i < level - 1; i++)
            levels[i].GetComponent<SpriteRenderer>().color = Color.gray;

        for (int i = level; i < levels.Count; i++)
            levels[i].SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int level = PlayerPrefs.GetInt("level");
            Vector2 screenPos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null && levels.IndexOf(hit.collider.gameObject) + 1 == level)
            {
                if (hit.collider.gameObject.name.StartsWith("camp"))
                    SceneManager.LoadScene("CampScene");
                else
                    SceneTransitionManager.Instance.LoadSceneWithCrossfade("BattleScene", true);
            }
        }
    }
}
