using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private DollyCartDestination dollyCartDestination;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private SettingsPanel settingsPanel;
    [SerializeField] private DeckPanel deckPanel;

    private Transform currentNode;
    private Vector3 currentNodeScale;
    private bool isNodeHovered = false;

    void Awake()
    {
        int level = SaveData.Level;
        levelText.text = level.ToString();

        for (int i = 0; i < level - 1; i++)
            levels[i].GetComponent<SpriteRenderer>().color = Palette.Get(ColorToken.StateInactive);

        for (int i = level; i < levels.Count; i++)
            levels[i].SetActive(false);

        currentNode = levels[level - 1].transform;
        currentNodeScale = currentNode.localScale;

        dollyCartDestination.Initialize();
        deckManager.Initialize();
        deckPanel.Initialize(deckManager);
        statusBar.Initialize(settingsPanel, deckPanel);
    }
    void Start()
    {
        statusBar.InitializeSlots(null);
    }
    void Update()
    {
        bool hovered = IsCurrentNodeUnderMouse();
        if (hovered != isNodeHovered)
            SetNodeHovered(hovered);

        if (hovered && Input.GetMouseButtonUp(0))
            EnterCurrentLevel();
    }

    private bool IsCurrentNodeUnderMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        return hit.collider != null && hit.collider.transform == currentNode;
    }

    private void SetNodeHovered(bool hovered)
    {
        isNodeHovered = hovered;
        currentNode.DOScale(hovered ? currentNodeScale * 1.2f : currentNodeScale, 0.15f);

        if (hovered)
            SfxManager.Instance.PlayFocusSound();
    }

    private void EnterCurrentLevel()
    {
        SfxManager.Instance.PlayClickSound();

        if (currentNode.name.StartsWith("camp"))
        {
            SceneManager.LoadScene(SceneName.Camp.ToSceneString());
        }
        else
        {
            SceneTransitionManager.Instance.LoadSceneWithCrossfade(SceneName.Battle);
        }
    }
}
