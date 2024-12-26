using UnityEngine;

public interface ILivability
{
    public int Livability
    {
        get;
    }
}
public class NursingHouse : ProductionBuilding, ILivability
{
    public int Livability => stationedCount != 0 ? level : 0;
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        LivabilityManager.Instance.Recalculate();
    }
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        LivabilityManager.Instance.Recalculate();
    }
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        LivabilityManager.Instance.livabilityBuildings.Add(this);
    }
    public override void OnDemolish()
    {
        base.OnDemolish();
        LivabilityManager.Instance.livabilityBuildings.Remove(this);
        LivabilityManager.Instance.Recalculate();
    }
}
