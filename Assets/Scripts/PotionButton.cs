using UnityEngine;

public class PotionButton : MonoBehaviour
{
    [SerializeField] private RewardPanel panel;

    public void OnClick()
    {
        panel.gameObject.SetActive(true);
        panel.ShowPotion();
    }
}
