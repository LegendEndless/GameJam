public class Magnetic : BaseBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        if (functioning)
        {
            LandscapeManager.Instance.magnetics.Add(position);
            LandscapeManager.Instance.RecalculateVisibility();
        }
        else
        {
            LandscapeManager.Instance.magnetics.Remove(position);
            LandscapeManager.Instance.RecalculateVisibility();
        }
    }
}
