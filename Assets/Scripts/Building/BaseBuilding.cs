using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    new public string name;
    public int level = 0;
    public Vector2Int position;
    public Vector2Int span;
    public int stationedCount;

    public bool upgrading = false;
    //写成两个参数，如果想把升级进度画成环形进度条的话会很方便
    public float timeSinceUpgrade;
    public float currentUpgradeDuration;

    public BuildingInfoPro buildingInfoPro;
    public Dictionary<Vector2Int, BaseBuilding> register;

    
    //有点多余吗。。至少提醒一下挂脚本或者实例化预制体时有哪些参数需要初始化
    public virtual void Initialize(string name,Vector2Int position,Vector2Int span)
    {
        this.name = name;
        this.position = position;
        this.span = span;

        register = BuildingManager.Instance.landUseRegister;

        Vector2Int v;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                register[v] = this;
            }
        }
        buildingInfoPro = BuildingManager.Instance.buildingInfoDict[name];

        level = 0;
        StartUpgrade();
    }
    public void StartUpgrade()
    {
        upgrading=true;
        timeSinceUpgrade = 0;
        currentUpgradeDuration = buildingInfoPro.buildingInfo.upgradeDuration[level];
    }
    public virtual void FinishUpgrade()
    {
        ++level;
        upgrading = false;
        ReportUpgrade();
        AutoAdjustStation();
    }
    private void Update()
    {
        if (timeSinceUpgrade >= currentUpgradeDuration)
        {
            FinishUpgrade();
        }
        if (upgrading)
        {
            timeSinceUpgrade += Time.deltaTime;
        }
    }
    public void Demolish()
    {
        Vector2Int v;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                register[v] = null;
            }
        }
        ManuallyAdjustStation(-stationedCount);
        Destroy(gameObject);
    }
    public void PopUI()
    {

    }
    public virtual void RecalculateMultiplier(bool causedByNeighbor)
    {

    }

    //这个函数方便做弹出UI的派驻功能
    public int StationedMax()
    {
        return Mathf.Min((level + 5), Mathf.FloorToInt(ResourceManager.Instance.GetResourceCount("PeopleAvailable")));
    }

    public void AutoAdjustStation()
    {
        int lastCount = stationedCount;
        //默认给你上满
        stationedCount = StationedMax();
        if (stationedCount == 0)
        {
            //没人了可咋办？按理不会在自动派遣时出现啦
        }
        if (stationedCount != lastCount)
        {
            RecalculateMultiplier(false);
        }
    }
    public void ManuallyAdjustStation(int delta)
    {
        stationedCount += delta;
        ResourceManager.Instance.AddResource("PeopleAvailable", -delta);
        RecalculateMultiplier(false);
    }
    public bool Search(string name)
    {
        Vector2Int u,v;
        int t;
        for (int i = 0;i < span.x; i++)
        {
            for(int j = 0;j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                for(int ii = -3; ii <= 3; ++ii)
                {
                    t = 3 - Mathf.Abs(ii);
                    for(int jj =-t; jj<=t; ++jj)
                    {
                        u = v + new Vector2Int(ii, jj);
                        if (register.ContainsKey(u) && register[u].name == name)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public virtual void ReportUpgrade()
    {

    }
}
