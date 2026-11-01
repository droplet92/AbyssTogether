using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private DollyCartDestination dollyCartDestination;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private StatusBar statusBar;

    void Awake()
    {
        int level = SaveData.Level;
        levelText.text = level.ToString();

        for (int i = 0; i < level - 1; i++)
            levels[i].GetComponent<SpriteRenderer>().color = Color.gray;

        for (int i = level; i < levels.Count; i++)
            levels[i].SetActive(false);

        dollyCartDestination.Initialize();
        deckManager.Initialize();
        statusBar.Initialize();
    }
    void Start()
    {
        statusBar.InitializeSlots();
    }
    void Update()
    {
        if (!Input.GetMouseButtonUp(0)) return;
        
        Vector2 screenPos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider == null) return;
        
        int level = SaveData.Level;
        if (levels.IndexOf(hit.collider.gameObject) + 1 != level) return;

        if (hit.collider.gameObject.name.StartsWith("camp"))
        {
            SceneManager.LoadScene(SceneName.Camp.ToSceneString());
        }
        else
        {
            SceneTransitionManager.Instance.LoadSceneWithCrossfade(SceneName.Battle);
        }
    }
}
