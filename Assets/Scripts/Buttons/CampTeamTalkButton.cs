using UnityEngine;

public class CampTeamTalkButton : BaseButton
{
    [SerializeField] private GameObject teamTalkPanel;

    public override void OnClick()
    {
        base.OnClick();
        teamTalkPanel.SetActive(true);
    }
}
