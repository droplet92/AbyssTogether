public interface ITarget
{
    public bool CanApply(string targetType);
    public void SetHighlight(bool isActive);
    public bool IsDead();
}
