using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFactory : ProductionBuilding
{
    bool functioning;
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        BuildingManager.Instance.globalMultiplier += (functioning ? 1 : -1) * level * 0.03f;//Ӳ����
        BuildingManager.Instance.GloballyRecalculate();
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        if (functioning)
        {
            BuildingManager.Instance.globalMultiplier += 0.03f;//Ӳ����
            BuildingManager.Instance.GloballyRecalculate();
        }
    }
}
