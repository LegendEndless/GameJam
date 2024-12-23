using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    static BuildingManager instance;
    public static BuildingManager Instance=>instance;
    public SerializableDictionary<string, BuildingInfoPro> buildingInfoDict;
    public Dictionary<string, int> buildingCountDict;
    public Dictionary<string, float> totalProduction;
    public Dictionary<string, int> highestLevel;
    //这样写不用确定地图大小，也能接受异形地图
    public Dictionary<Vector2Int, BaseBuilding> landUseRegister;

    public float globalMultiplier;
    private void Awake()
    {
        instance = this;
        BuildingInfoCollection collection = XmlDataManager.Instance.Load<BuildingInfoCollection>("building");
        buildingInfoDict = new SerializableDictionary<string, BuildingInfoPro>();
        foreach (BuildingInfo info in collection.buildingInfos)
        {
            BuildingInfoPro t = new BuildingInfoPro(info);
            buildingInfoDict[info.name] = t;
        }
        totalProduction = new Dictionary<string, float>
        {
            {"Electricity", 0},
            {"Minerals", 0},
            {"Food", 0},
            {"Water", 0},
            {"Oil", 0},
            {"Chips", 0},
            {"Alloy", 0},
            {"Fibre", 0},
        };
        landUseRegister = new Dictionary<Vector2Int, BaseBuilding>();
        highestLevel = new Dictionary<string, int>();
        buildingCountDict = new Dictionary<string, int>();
        globalMultiplier = 0;
    }
    private void Start()
    {
        
    }
    public void ReportMultiplierChange(ProductionBuilding building, float deltaMultiplier)
    {
        Dictionary<string, float> basicProduction = building.buildingInfoPro.massProductionList[building.level - 1];
        foreach(KeyValuePair<string,float> pair in basicProduction)
        {
            totalProduction[pair.Key] += pair.Value * deltaMultiplier;
        }
    }
    public void ReportUpgrade(ProductionBuilding building)
    {
        if(building.multiplier == 0)
        {
            return;
        }
        Dictionary<string, float> currentProduction = building.buildingInfoPro.massProductionList[building.level - 1];
        Dictionary<string, float> formerProduction = building.buildingInfoPro.massProductionList[building.level - 2];
        float current, former;
        foreach(string resource in totalProduction.Keys)
        {
            current = currentProduction.ContainsKey(resource) ? currentProduction[resource] : 0;
            former = formerProduction.ContainsKey(resource) ? formerProduction[resource] : 0;
            totalProduction[resource] += building.multiplier * (current - former);
        }
    }
    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<string,float> pair in totalProduction)
        {
            ResourceManager.Instance.AddResource(pair.Key, pair.Value * Time.deltaTime);
        }
    }
}
