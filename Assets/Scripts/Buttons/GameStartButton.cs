using BgmType = BgmManager.BgmType;

public class GameStartButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        BgmManager.Instance.CrossFade(BgmType.Opening, BgmType.NonBattle);
        SceneTransitionManager.Instance.LoadSceneWithCrossfade("CharacterSelectScene");
    }
}
