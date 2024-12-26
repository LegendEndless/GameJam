public class RobotFactory : ProductionBuilding
{
    bool functioning;
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        BuildingManager.Instance.globalMultiplier += (functioning ? 1 : -1) * level * 0.03f;//Ó²±àÂë
        BuildingManager.Instance.GloballyRecalculate();
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        if (functioning)
        {
            BuildingManager.Instance.globalMultiplier += 0.03f;//Ó²±àÂë
            BuildingManager.Instance.GloballyRecalculate();
        }
    }
    public override void OnDemolish()
    {
        base.OnDemolish();

    }
}
