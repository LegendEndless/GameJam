using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apartment : BaseBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        PopulationManager.Instance.maxPopulation += functioning ? 20 : -20;//Ó²±àÂë
    }
}
