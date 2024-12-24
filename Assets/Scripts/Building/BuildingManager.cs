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
    public Dictionary<string, BaseBuilding> highestLevelBuilding;
    public HashSet<BaseBuilding> buildings;
    public Dictionary<string, bool> freeDict;
    //这样写不用确定地图大小，也能接受异形地图
    public Dictionary<Vector2Int, BaseBuilding> landUseRegister;

    public int AITimesLeft;
    public float AIMultiplier;

    public float globalMultiplier;

    public bool rocketBaseFunctioning;
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
        highestLevelBuilding = new Dictionary<string, BaseBuilding>();
        buildingCountDict = new Dictionary<string, int>();
        globalMultiplier = 0;
        buildings = new HashSet<BaseBuilding>();
        freeDict = new Dictionary<string, bool>();
        AITimesLeft = 3;
        AIMultiplier = 0;
        rocketBaseFunctioning = false;
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
    public void GloballyRecalculate()
    {
        foreach(BaseBuilding building in buildings)
        {
            if(building is ProductionBuilding)
            {
                (building as ProductionBuilding).RecalculateMultiplier(false,true);
            }
        }
    }
    public void UpdateHighestLevel(string name)
    {
        foreach (BaseBuilding building in buildings)
        {
            if(building.name == name && (highestLevelBuilding[name]==null || highestLevelBuilding[name].level < building.level))
            {
                highestLevelBuilding[name] = building;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<string,float> pair in totalProduction)
        {
            ResourceManager.Instance.AddResource(pair.Key, pair.Value * Time.deltaTime);
            if(ResourceManager.Instance.GetResourceCount(pair.Key) < 0)
            {
                ResourceManager.Instance.AddResource(pair.Key, -ResourceManager.Instance.GetResourceCount(pair.Key));
                //To do:可以通知一下玩家 有资源归零，所有消耗该资源的建筑都已停工
                DismissAllRelatedBuilding(pair.Key);
            }
        }
    }
    public void DismissAllRelatedBuilding(string resource)
    {
        foreach(var building in buildings)
        {
            if (building.buildingInfoPro.massProductionList[building.level-1].ContainsKey(resource) && building.buildingInfoPro.massProductionList[building.level - 1][resource] < 0)
            {
                building.ManuallyAdjustStation(-building.stationedCount);
            }
        }
    }
}
