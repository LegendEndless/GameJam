public class RocketBase : BaseBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        BuildingManager.Instance.rocketBaseFunctioning = functioning;
    }
}
