using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    static BuildingManager instance;
    public static BuildingManager Instance=>instance;
    public SerializableDictionary<string, BuildingInfo> buildingInfoDict;
    public Dictionary<string, int> buildingCountDict;
    //这样写不用确定地图大小，也能接受异形地图
    public Dictionary<Vector2Int, bool> landUseRegister;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //读取建筑配置表到buildingInfoDict
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
