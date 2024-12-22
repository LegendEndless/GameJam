using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    static BuildingManager instance;
    public static BuildingManager Instance=>instance;
    public SerializableDictionary<string, BuildingInfoPro> buildingInfoDict;
    public Dictionary<string, int> buildingCountDict;
    //这样写不用确定地图大小，也能接受异形地图
    public Dictionary<Vector2Int, bool> landUseRegister;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BuildingInfoCollection collection = XmlDataManager.Instance.Load<BuildingInfoCollection>("building");
        buildingInfoDict = new SerializableDictionary<string, BuildingInfoPro>();
        foreach(BuildingInfo info in collection.buildingInfos)
        {
            BuildingInfoPro t = new BuildingInfoPro(info);
            buildingInfoDict[info.name] = t;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
