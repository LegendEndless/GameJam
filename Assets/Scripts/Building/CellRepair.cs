using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRepair : BaseBuilding
{
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        if (stationedCount > 0)
        {
            PopulationManager.Instance.rate += 0.05f;
        }
    }
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        PopulationManager.Instance.rate += functioning ? level * 0.05f : - level * 0.05f;//Ó²±àÂë
    }
}
