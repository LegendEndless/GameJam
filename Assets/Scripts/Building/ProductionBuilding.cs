using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    public float multiplier;
    public float neighborMultiplier;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        multiplier = 0;
        neighborMultiplier = -1;
    }
    public override void AutoAdjustStation()
    {
        ManuallyAdjustStation(StationedMax() - stationedCount);
    }
    public override void ManuallyAdjustStation(int delta)
    {
        base.ManuallyAdjustStation(delta);
        RecalculateMultiplier(false);
    }
    public void RecalculateMultiplier(bool causedByNeighbor)
    {
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
            //这儿还得再看一下有没有那个通用增幅器
            if (LandBuffed())//To do:记得再判断一下是不是需要考虑通用增幅的建筑种类
            {
                neighborMultiplier += 0.2f;//某个硬编码的增幅值
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
    public bool LandBuffed()
    {
        Vector2Int v;
        Dictionary<Vector2Int,bool> dic = BuildingManager.Instance.landBuff;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                if (dic.ContainsKey(v) && dic[v])
                {
                    return true;
                }
            }
        }
        return false;
    }
}
