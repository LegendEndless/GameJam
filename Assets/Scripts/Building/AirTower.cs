using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTower : BaseBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        if (functioning)
        {
            LandscapeManager.Instance.airTowers.Add(position);
            LandscapeManager.Instance.RecalculateBuildability();
        }
        else
        {
            LandscapeManager.Instance.airTowers.Remove(position);
            LandscapeManager.Instance.RecalculateBuildability();
        }
    }
}