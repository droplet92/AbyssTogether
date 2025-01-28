using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Monster : AutoFieldValidator, ICardTarget
{
    [Header("몬스터 기본 설정")]
    [SerializeField] private Image body;
    [SerializeField] private Image bodyHighlight;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TMP_Text hpDiff;
    [SerializeField] public Buffs buffs;
    [SerializeField] private Image nextAction;
    [SerializeField] private List<Character> characterList;
    
    [Header("공격용")]
    [SerializeField] private FieldManager fieldManager;
    [SerializeField] private HandUI handPanel;
    
    private int maxHealth = 0;
    private int currentHealth = 0;
    private int attack = 0;
    private int defense = 0;
    private List<string> actions;

    public int Attack { get => attack; }
    public int Defense { get => defense; }

    public bool CanApply(string targetType)
    {
        if (targetType == "Monster") return true;
        if (targetType == "MonsterAll") return true;
        return false;
    }
    public void SetHighlight(bool isActive)
    {
        if (bodyHighlight != null)
            bodyHighlight.gameObject.SetActive(isActive);
    }
    public bool isDied()
    {
        return currentHealth <= 0;
    }

    public void Initialize(MonsterData data)
    {
        maxHealth = currentHealth = data.health;
        attack = data.attack;
        defense = data.defense;
        actions = new List<string>(data.actions);

        body.sprite = data.body;
        bodyHighlight.sprite = data.bodyHightlight;
        nextAction.sprite = Resources.Load<Sprite>($"Images/Actions/{actions.First().ToLower()}");
        
        healthBar.Initialize(maxHealth);
    }
    public IEnumerator DoAction()
    {
        string nextActionString = actions.First();
        var target = GetRandomCharacter();
        
        if (target != null)
        {
            fieldManager.ApplyEffect("Character", this, target, nextActionString);
            actions.RemoveAt(0);
            actions.Add(nextActionString);
            nextAction.sprite = Resources.Load<Sprite>($"Images/Actions/{actions.First().ToLower()}");
        }
        yield return new WaitForSeconds(1f);
    }

    public void UpdateHealth(int delta)
    {
        int value = math.min(delta, maxHealth);
        currentHealth = math.min(currentHealth + value, maxHealth);

        hpDiff.gameObject.SetActive(true);
        hpDiff.text = value.ToString();

        healthBar.SetHealth(currentHealth, maxHealth);

        if (value < 0)
        {
            transform.DOShakePosition
            (
                duration: 0.5f,
                strength: 10f,
                vibrato: 100,
                randomness: 90,
                fadeOut: true
            );
            hpDiff.color = new Color32(0xBC, 0x01, 0x19, 0xFF);
        }
        else if (value > 0)
            hpDiff.color = new Color32(0x31, 0x62, 0x57, 0xFF);
        else
            hpDiff.color = Color.white;

        var localY = hpDiff.transform.localPosition.y;

        DOTween.Sequence()
            .Join(hpDiff.transform.DOLocalMoveY(localY + 30f, 0.5f))
            .Join(hpDiff.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                hpDiff.transform.DOLocalMoveY(localY, 0);
                hpDiff.gameObject.SetActive(false);
            });
    }
    public void UpdateAttack(int delta)
    {
        attack = math.max(attack + delta, 0);
        buffs.UpdateContents(delta, 0);
    }
    public void UpdateDefense(int delta)
    {
        defense = math.max(defense + delta, 0);
        buffs.UpdateContents(0, delta);
    }
    
    public void AnimateAttack()
    {
        var rectTransform = body.GetComponent<RectTransform>();

        rectTransform.DOPunchAnchorPos(new Vector2(-100f, 0), 0.5f, 6)
            .SetEase(Ease.InExpo);
    }
    public void AnimateSpell()
    {
        var rectTransform = GetComponent<RectTransform>();

        rectTransform.DOJump(rectTransform.position, 10f, 3, 0.5f)
            .SetEase(Ease.InQuad);
    }
    public void AnimateHit()
    {
        var rectTransform = body.GetComponent<RectTransform>();

        rectTransform.DOPunchAnchorPos(new Vector2(60f, 0), 0.5f, 5)
            .SetEase(Ease.InQuart);
    }

    private Character GetRandomCharacter()
    {
        characterList.RemoveAll(x => x.isDied());

        if (characterList.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, characterList.Count);
        return characterList[randomIndex];
    }
}
