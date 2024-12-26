using System.Collections.Generic;
using UnityEngine;

public class Gym : ProductionBuilding, ILivability
{
    public List<float> ranges = new List<float>
    {
        2.24f,2.45f,2.65f,2.83f,3.01f
    };//Ӳ����
    public int Livability => stationedCount != 0 ? CountInRange(null, ranges[level - 1]) : 0;
    public override void AutoAdjustStation()
    {
        if (stationedCount == 0 && PopulationManager.Instance.AvailablePopulation <= 0)
        {
            //û����פ��ɶҲ����
            return;
        }
        //������Դ������Ĭ����1��
        ManuallyAdjustStation(1 - stationedCount);
    }
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
