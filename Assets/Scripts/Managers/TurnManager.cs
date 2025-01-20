using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private HandUI handUI;
    [SerializeField] private GameObject turnEndButton;
    [SerializeField] private List<Character> characterList;
    [SerializeField] private List<Monster> monsterList;

    private int nDraws = 4;

    void Start()
    {
        int level = PlayerPrefs.GetInt("level");
        levelText.text = level.ToString();

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

            resultPanel.gameObject.SetActive(true);
            resultPanel.ShowWin();
            gameObject.SetActive(false);
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
        if (isOver)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.ShowDefeat();
            gameObject.SetActive(false);
        }
    }
    public void EndTurn()
    {
        StartCoroutine(EndTurnRoutine());
    }
    public void ReduceDraw(int n)
    {
        nDraws -= n;
    }
    private IEnumerator StartTurn()
    {
        turnEndButton.SetActive(false);

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
