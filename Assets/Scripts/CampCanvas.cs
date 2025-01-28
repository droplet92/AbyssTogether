using UnityEngine;
using UnityEngine.UI;

public class CampCanvas : AutoFieldValidator
{
    [SerializeField] private CampManager campManager;
    [SerializeField] private RewardPanel rewardPanel;
    [SerializeField] private GameObject teamTalkPanel;
    [SerializeField] private Button restButton;
    [SerializeField] private Button searchAreaButton;
    [SerializeField] private Button teamTalkButton;

    void Awake()
    {
        restButton.onClick.AddListener(Rest);
        searchAreaButton.onClick.AddListener(OpenRewardPanel);
        teamTalkButton.onClick.AddListener(OpenTeamTalkPanel);

        restButton.onClick.AddListener(() => campManager.ExitCamp());
        rewardPanel.AddConfirmListener(() => campManager.ExitCamp());
    }

    private void Rest()
    {
        int healerHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpHealer") * 1.2f));
        int magicianHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpMagician") * 1.2f));
        int swordsmanHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpSwordsman") * 1.2f));
        int warriorHp = Mathf.Min(30, Mathf.RoundToInt(PlayerPrefs.GetInt("HpWarrior") * 1.2f));
        
        PlayerPrefs.SetInt($"HpHealer", healerHp);
        PlayerPrefs.SetInt($"HpMagician", magicianHp);
        PlayerPrefs.SetInt($"HpSwordsman", swordsmanHp);
        PlayerPrefs.SetInt($"HpWarrior", warriorHp);
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
