using System.Collections.Generic;
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [SerializeField] private List<Character> characters;
    [SerializeField] private MonsterManager monsterManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private FieldManager fieldManager;
    [SerializeField] private StatusBar statusBar;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private RewardPanel rewardPanel;
    [SerializeField] private TurnManager turnManager;

    void Awake()
    {
        foreach (var character in characters)
            character.Initialize();

        monsterManager.Initialize();
        deckManager.Initialize();
        statusBar.Initialize();
        resultPanel.Initialize();
        rewardPanel.Initialize();
        turnManager.Initialize();
    }
    void Start()
    {
        fieldManager.Initialize();
        statusBar.InitializeSlots();
    }
}
