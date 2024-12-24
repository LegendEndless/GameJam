using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBase : BaseBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        BuildingManager.Instance.rocketBaseFunctioning = functioning;
    }
}
