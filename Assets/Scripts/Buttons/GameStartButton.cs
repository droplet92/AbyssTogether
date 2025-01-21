public class GameStartButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        BgmManager.Instance.StopOpeningBGM();
        SceneTransitionManager.Instance.LoadSceneWithCrossfade("CharacterSelectScene", false);
    }
}
