public interface ICardTarget
{
    public bool CanApply(string targetType);
    public void SetHighlight(bool isActive);
    public bool isDied();
}
