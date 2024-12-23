using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFactory : ProductionBuilding
{
    bool functioning;
    public override void ManuallyAdjustStation(int delta)
    {
        base.ManuallyAdjustStation(delta);
        if (functioning != (stationedCount != 0))
        {
            functioning = (stationedCount != 0);
            //֪ͨBuildingManager��ȫ�ּӳ����б仯
            BuildingManager.Instance.globalMultiplier += (functioning ? 1 : -1) * level * 0.03f;//Ӳ����
        }
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        if (functioning)
        {
            BuildingManager.Instance.globalMultiplier += 0.03f;//Ӳ����
        }
    }
}
