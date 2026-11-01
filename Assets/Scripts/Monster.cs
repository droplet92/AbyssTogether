using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, ITarget
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

    public int CurrentHealth => unit?.CurrentHealth ?? 0;
    public int Attack => unit?.Attack ?? 0;
    public int Defense => unit?.Defense ?? 0;

    private Unit unit;
    private List<string> actions;
    private Dictionary<string, Sprite> actionSpriteCache = new Dictionary<string, Sprite>();

    public bool CanApply(string targetType)
    {
        if (IsDead()) return false;
        if (targetType == "Monster") return true;
        if (targetType == "MonsterAll") return true;
        return false;
    }
    public void SetHighlight(bool isActive)
    {
        if (IsDead()) return;
        bodyHighlight?.gameObject.SetActive(isActive);
    }
    public bool IsDead()
    {
        return unit == null || unit.IsDead;
    }

    public void Initialize(MonsterData data)
    {
        buffs.Initialize();
        unit = new Unit(data.health, data.health, data.attack, data.defense);
        actions = new List<string>(data.actions);   // copy

        body.sprite = data.body;
        bodyHighlight.sprite = data.bodyHightlight;
        nextAction.sprite = GetActionSprite(actions.First());
        healthBar.Initialize(unit.MaxHealth);
    }
    public IEnumerator DoAction()
    {
        if (IsDead()) yield break;
        var target = GetRandomCharacter();
        if (target != null)
        {
            (var curr, var next) = GetNextAction();
            fieldManager.ApplyEffect("Character", this, target, curr);
            nextAction.sprite = GetActionSprite(next);
        }
        yield return new WaitForSeconds(1f);
    }

    public void UpdateHealth(int delta)
    {
        int? applied = unit?.ChangeHealth(delta);
        if (applied == null) return;

        int deltaHp = applied.Value;
        healthBar.SetHealth(unit.CurrentHealth, unit.MaxHealth);

        hpDiff.text = deltaHp.ToString();
        hpDiff.color = GetHpDiffColor(deltaHp);
        hpDiff.gameObject.SetActive(true);

        if (deltaHp < 0)
            transform.DOShakePosition(0.5f, 10f, 100);

        float hpDiffBeginY = hpDiff.transform.localPosition.y;
        DOTween.Sequence()
            .Join(hpDiff.transform.DOLocalMoveY(hpDiffBeginY + 30f, 0.5f))
            .Join(hpDiff.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                hpDiff.transform.DOLocalMoveY(hpDiffBeginY, 0);
                hpDiff.gameObject.SetActive(false);
            });
    }
    public void UpdateAttack(int delta)
    {
        int? change = unit?.ChangeAttack(delta);
        if (change == null) return;

        buffs.UpdateContents(change.Value, 0);
    }
    public void UpdateDefense(int delta)
    {
        int? change = unit?.ChangeDefense(delta);
        if (change == null) return;

        buffs.UpdateContents(0, change.Value);
    }
    
    public void AnimateAttack()
    {
        if (IsDead()) return;
        body.GetComponent<RectTransform>()
            .DOPunchAnchorPos(new Vector2(-100f, 0), 0.5f, 6)
            .SetEase(Ease.InExpo);
    }
    public void AnimateSpell()
    {
        if (IsDead()) return;
        transform.DOJump(transform.position, 10f, 3, 0.5f)
            .SetEase(Ease.InQuad);
    }
    public void AnimateHit()
    {
        if (IsDead()) return;
        body.GetComponent<RectTransform>()
            .DOPunchAnchorPos(new Vector2(60f, 0), 0.5f, 5)
            .SetEase(Ease.InQuart);
    }

    private Sprite GetActionSprite(string action)
    {
        if (!actionSpriteCache.ContainsKey(action))
            actionSpriteCache[action] = Resources.Load<Sprite>($"Images/Actions/{action.ToLower()}");
        return actionSpriteCache[action];
    }
    private Color32 GetHpDiffColor(int delta)
    {
        if (delta < 0) return new Color32(0xBC, 0x01, 0x19, 0xFF);  // red
        if (delta > 0) return new Color32(0x31, 0x62, 0x57, 0xFF);  // green
        return Color.white;
    }
    private Character GetRandomCharacter()
    {
        characterList.RemoveAll(x => x.IsDead());
        if (characterList.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, characterList.Count);
        return characterList[randomIndex];
    }
    private KeyValuePair<string, string> GetNextAction()
    {
        string curr = actions.First();
        actions.RemoveAt(0);
        actions.Add(curr);
        string next = actions.First();
        return new KeyValuePair<string, string>(curr, next);
    }
}
