using UnityEngine;

public class EcoGarden : BaseBuilding, ILivability
{
    public int Livability => throw new System.NotImplementedException();
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
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
