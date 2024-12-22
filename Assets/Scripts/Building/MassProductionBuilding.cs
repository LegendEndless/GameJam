using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassProductionBuilding : BaseBuilding
{
    public float multiplier;
    public float neighborMultiplier;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        multiplier = 0;
        neighborMultiplier = -1;
    }
    public override void RecalculateMultiplier(bool causedByNeighbor)
    {
        base.RecalculateMultiplier(causedByNeighbor);
        float lastMultiplier = multiplier;
        if (causedByNeighbor || neighborMultiplier == -1)
        {
            neighborMultiplier = 0;
            foreach (KeyValuePair<string, float> pair in buildingInfoPro.neighborBonusDict)
            {
                if (Search(pair.Key))
                {
                    neighborMultiplier += pair.Value;
                }
            }
        }
        if (stationedCount == 0)
        {
            multiplier = 0;
        }
        else
        {
            multiplier = 1f;
            multiplier += (stationedCount - 1) * buildingInfoPro.buildingInfo.stationBonus;
            multiplier += neighborMultiplier;
        }
        BuildingManager.Instance.ReportMultiplierChange(this,multiplier-lastMultiplier);
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        BuildingManager.Instance.ReportUpgrade(this);
    }
}
