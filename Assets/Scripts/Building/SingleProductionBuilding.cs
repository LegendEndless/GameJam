using System.Collections.Generic;
using UnityEngine;

public class SingleProductionBuilding : ProductionBuilding
{
    Dictionary<string, float> localProduction;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        localProduction = new Dictionary<string, float>();
    }
    public override void Update()
    {
        base.Update();
        Dictionary<string, float> singleProduction = BuildingManager.Instance.buildingInfoDict[name].singleProductionList[level - 1];
        foreach (KeyValuePair<string, float> pair in singleProduction)
        {
            if (!localProduction.ContainsKey(pair.Key))
            {
                localProduction[pair.Key] = 0;
            }
            if (pair.Key == "chip_part")
            {
                localProduction[pair.Key] += (multiplier + BuildingManager.Instance.AIMultiplier) * pair.Value * Time.deltaTime;
            }
            else
            {
                localProduction[pair.Key] += multiplier * pair.Value * Time.deltaTime;
            }
            if (localProduction[pair.Key] > 1)
            {
                //需要什么生产完成特效的话往这儿加
                ResourceManager.Instance.AddResource(pair.Key, 1);
                --localProduction[pair.Key];
            }
        }
    }
}
