using UnityEngine;

public class PotionButton : BaseButton
{
    [SerializeField] private RewardPanel panel;

    public override void OnClick()
    {
        base.OnClick();
        panel.gameObject.SetActive(true);
        panel.ShowPotion();
    }
}
