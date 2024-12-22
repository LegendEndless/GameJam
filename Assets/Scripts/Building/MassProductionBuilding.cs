using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassProductionBuilding : BaseBuilding
{
    public float production;
    public override void RecalculateProduction()
    {
        base.RecalculateProduction();
        production = buildingInfoPro.buildingInfo.production[level-1];//这个-1标记一下
        //再计算人口进驻和邻近建筑的bonus
        production += buildingInfoPro.buildingInfo.stationBonus * stationedCount;
        
    }
}
