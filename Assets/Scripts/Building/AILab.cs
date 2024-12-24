using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILab : BaseBuilding
{
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        if(BuildingManager.Instance.AITimesLeft > 0)
        {
            --BuildingManager.Instance.AITimesLeft;
            ResourceManager.Instance.AddResource("chip",ResourceManager.Instance.GetResourceCount("chip")/2);
        }
    }
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        BuildingManager.Instance.AIMultiplier += functioning ? 0.1f : -0.1f;
    }
}