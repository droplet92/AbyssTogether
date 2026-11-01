using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    [SerializeField] private RewardPanel rewardPanel;
    [SerializeField] private GameObject teamTalkPanel;
    [SerializeField] private Button restButton;
    [SerializeField] private Button searchAreaButton;
    [SerializeField] private Button teamTalkButton;
    [SerializeField] private Image fadeOut;
    [SerializeField] private TMP_Text levelText;

    private int level = 0;

    void Awake()
    {
        level = SaveData.Level;
        levelText.text = level.ToString();

        restButton.onClick.AddListener(Rest);
        searchAreaButton.onClick.AddListener(OpenRewardPanel);
        teamTalkButton.onClick.AddListener(OpenTeamTalkPanel);

        restButton.onClick.AddListener(() => ExitCamp());
        rewardPanel.AddConfirmListener(() => ExitCamp());
    }

    public void ExitCamp()
    {
        SaveData.Level = level + 1;

        fadeOut.gameObject.SetActive(true);
        fadeOut.DOFade(1f, 1f)
            .OnComplete(() => SceneManager.LoadScene(SceneName.Level.ToSceneString()));
    }

    private void Rest()
    {
        foreach (var member in SaveData.PartyMembers)
        {
            int hp = SaveData.GetHealth(member);
            if (hp <= 0) continue;

            SaveData.SetHealth(member, Mathf.Min(hp + 6, SaveData.MaxHealth));
        }
    }
    private void OpenRewardPanel()
    {
        rewardPanel.gameObject.SetActive(true);
        rewardPanel.ShowItem();
    }
    private void OpenTeamTalkPanel()
    {
        teamTalkPanel.SetActive(true);
    }
}
