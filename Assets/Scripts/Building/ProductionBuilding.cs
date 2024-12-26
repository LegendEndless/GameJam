using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    public float multiplier;
    public float environmentMultiplier;
    public float globalMultiplier;
    int numLAN;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        multiplier = 0;
        environmentMultiplier = -1;
        globalMultiplier = -1;
        //刚开始就得统计一下周边基站个数
        numLAN = CountInRange("LAN", 2.01f);//硬编码处
        if (name == "MantleSampling")
        {
            BuildingManager.Instance.sampling[LandscapeManager.Instance.landscapeMap[position]] = true;
        }
    }
    public override void AutoAdjustStation()
    {
        ManuallyAdjustStation(StationedMax() - stationedCount);
    }
    public override void ManuallyAdjustStation(int delta)
    {
        base.ManuallyAdjustStation(delta);
        RecalculateMultiplier(false, false);
    }
    public void RecalculateMultiplier(bool recalcEnvironment, bool recalcGlobal)
    {
        float lastMultiplier = multiplier;
        if (recalcGlobal || globalMultiplier == -1)
        {
            if (buildingInfoPro.buildingInfo.group == 1 || buildingInfoPro.buildingInfo.group == 4)
            {
                globalMultiplier = BuildingManager.Instance.globalMultiplier + LivabilityManager.Instance.livability * 0.02f;
            }
            else
            {
                globalMultiplier = 0;
            }
        }
        if (recalcEnvironment || environmentMultiplier == -1)
        {
            environmentMultiplier = 0;
            foreach (KeyValuePair<string, float> pair in buildingInfoPro.neighborBonusDict)
            {
                if (HasNeighbor(pair.Key))
                {
                    environmentMultiplier += pair.Value;
                }
            }
            if (buildingInfoPro.buildingInfo.group == 1 || buildingInfoPro.buildingInfo.group == 4)
            {
                if (numLAN > 0)
                {
                    environmentMultiplier += 0.2f;//某个硬编码的增幅值
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
            multiplier += environmentMultiplier;
            multiplier += globalMultiplier;
        }
        BuildingManager.Instance.ReportMultiplierChange(this, multiplier - lastMultiplier);
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        BuildingManager.Instance.ReportUpgrade(this);
    }
    public void ChangeNumLAN(int change)
    {
        numLAN += change;
        RecalculateMultiplier(true, false);
    }
    public override void OnDemolish()
    {
        base.OnDemolish();
        if (name == "MantleSampling")
        {
            BuildingManager.Instance.sampling[LandscapeManager.Instance.landscapeMap[position]] = false;
        }
    }
}
