using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup nextTurn;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private HandUI handUI;
    [SerializeField] private GameObject turnEndButton;
    [SerializeField] private List<Character> characterList;
    [SerializeField] private List<Monster> monsterList;

    private int nDraws = 4;
    private int playerCharacter;

    void Awake()
    {
        int level = PlayerPrefs.GetInt("level");
        levelText.text = level.ToString();
        playerCharacter = PlayerPrefs.GetInt("PlayerCharacter") - 1;

        turnEndButton.SetActive(false);
        StartCoroutine(StartTurn());
    }
    void Update()
    {
        bool isOver = true;

        foreach (var monster in monsterList)
        {
            if (monster.gameObject.activeSelf || !monster.isDied())
            {
                isOver = false;
                break;
            }
        }
        if (isOver)
        {
            foreach (var character in characterList)
                PlayerPrefs.SetInt($"Hp{character.gameObject.name}", character.CurrentHealth);

            if (levelText.text == "17")
            {
                gameObject.SetActive(false);
                SceneTransitionManager.Instance.LoadSceneWithCrossfade(SceneName.Ending);
            }
            else
            {
                resultPanel.gameObject.SetActive(true);
                resultPanel.ShowWin();
                gameObject.SetActive(false);
            }
        }
        isOver = true;

        foreach (var character in characterList)
        {
            if (character.gameObject.activeSelf || !character.isDied())
            {
                isOver = false;
                break;
            }
        }
        bool isPlayerDied = !characterList[playerCharacter].gameObject.activeSelf || characterList[playerCharacter].isDied();
        if (isOver || isPlayerDied)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.ShowDefeat();
            gameObject.SetActive(false);
        }
    }
    public void EndTurn()
    {
        turnEndButton.SetActive(false);
        StartCoroutine(EndTurnRoutine());
    }
    public void ReduceDraw(int n)
    {
        nDraws -= n;
    }
    private IEnumerator StartTurn()
    {
        nextTurn.gameObject.SetActive(true);
        nextTurn.DOFade(1f, 0.2f);
        yield return new WaitForSeconds(1.5f);

        nextTurn.DOFade(0f, 0.2f)
            .OnComplete(() => nextTurn.gameObject.SetActive(false));

        foreach (var character in characterList)
            character.UpdateDefense(-character.Defense);

        for (int i = 0; i < nDraws; i++)
        {
            handUI.DrawCard();
            yield return new WaitForSeconds(0.5f);
        }
        nDraws = 4;
        turnEndButton.SetActive(true);
    }
    private IEnumerator EndTurnRoutine()
    {
        yield return StartCoroutine(handUI.DiscardAll());
        handUI.ClearHand();
        
        foreach (var monster in monsterList)
        {
            monster.UpdateDefense(-monster.Defense);

            if (!monster.isDied())
                yield return StartCoroutine(monster.DoAction());
        }
        yield return StartCoroutine(StartTurn());
    }
}
