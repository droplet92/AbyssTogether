using UnityEngine;

public class CampTeamTalkButton : MonoBehaviour
{
    [SerializeField] private GameObject teamTalkPanel;

    public void OnClick()
    {
        teamTalkPanel.SetActive(true);
    }
}
