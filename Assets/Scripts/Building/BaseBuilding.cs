using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    new public string name;
    public int level = 0;
    public Vector2Int position;
    public Vector2Int span;
    public int stationedCount;//注意写特殊建筑效果逻辑时也要看看有没有至少派驻一人

    public bool upgrading = false;
    //写成两个参数，如果想把升级进度画成环形进度条的话会很方便
    public float timeSinceUpgrade;
    public float currentUpgradeDuration;

    public BuildingInfoPro buildingInfoPro;
    public Dictionary<Vector2Int, BaseBuilding> register;

    public bool onStrike;//UI也得考虑这个
    public float strikeTimeLeft;
    public int stationedCountBeforeStrike;

    //有点多余吗。。至少提醒一下挂脚本或者实例化预制体时有哪些参数需要初始化
    public virtual void Initialize(string name,Vector2Int position,Vector2Int span)
    {
        this.name = name;
        this.position = position;
        this.span = span;

        onStrike = false;

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
        //建筑数量也记下来了
        var cd = BuildingManager.Instance.buildingCountDict;
        if (!cd.ContainsKey(name))
            cd[name] = 1;
        else
            ++cd[name];

        BuildingManager.Instance.buildings.Add(this);

        buildingInfoPro = BuildingManager.Instance.buildingInfoDict[name];

        level = 0;

        stationedCount = 0;

        LivabilityManager.Instance.eventLivability += buildingInfoPro.buildingInfo.livabilityBoost;
        LivabilityManager.Instance.Recalculate();

        foreach (var building in GetNeighborsInRange(3.01f))
        {
            if (building is ProductionBuilding && building != this)
                (building as ProductionBuilding).RecalculateMultiplier(true, false);
        }

        StartUpgrade();
    }
    public void StartUpgrade()
    {
        if (upgrading) return;
        if (level > 0 && BuildingManager.Instance.freeDict.ContainsKey(name) && BuildingManager.Instance.freeDict[name])
        {
            BuildingManager.Instance.freeDict[name] = false;
        }
        else
        {
            foreach (var t in buildingInfoPro.costList[level])
            {
                ResourceManager.Instance.AddResource(t.Key, -t.Value);
            }
        }
        upgrading =true;
        timeSinceUpgrade = 0;
        currentUpgradeDuration = buildingInfoPro.durationList[level];
    }
    public virtual void FinishUpgrade()
    {
        ++level;
        upgrading = false;

        ReportUpgrade();
        AutoAdjustStation();
    }
    public virtual void Update()
    {
        if (upgrading)
        {
            timeSinceUpgrade += Time.deltaTime;
            if (timeSinceUpgrade >= currentUpgradeDuration)
            {
                FinishUpgrade();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartUpgrade();
            }
        }
        if (onStrike)
        {
            strikeTimeLeft -= Time.deltaTime;
            if (strikeTimeLeft < 0)
            {
                onStrike = false;
                ManuallyAdjustStation(Mathf.Min(stationedCountBeforeStrike, PopulationManager.Instance.AvailablePopulation));
            }
        }
        
    }
    public void Demolish()
    {
        ManuallyAdjustStation(-stationedCount);
        RealTimeBuilder.Instance.Demolish(position);
        OnDemolish();
        Destroy(gameObject);
    }
    public virtual void OnDemolish()
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
        BuildingManager.Instance.buildings.Remove(this);
    }
    public void PopUI()
    {

    }

    //这个函数方便做弹出UI的派驻功能
    public int StationedMax()
    {
        return Mathf.Min((level + 5), Mathf.FloorToInt(PopulationManager.Instance.AvailablePopulation));
    }

    public virtual void AutoAdjustStation()
    {
        if(stationedCount == 0 && PopulationManager.Instance.AvailablePopulation <= 0)
        {
            //没人派驻，啥也不做
            return;
        }
        //不是资源建筑就默认上1吧
        ManuallyAdjustStation(1-stationedCount);
    }
    public virtual void ManuallyAdjustStation(int delta)
    {
        bool flag = stationedCount > 0;
        if(delta==0)
        {
            return;
        }
        stationedCount += delta;
        PopulationManager.Instance.stationedPopulation += delta;
        flag ^= (stationedCount > 0);
        if (flag)
        {
            OnFunctioningChange(stationedCount > 0);
        }
    }
    public virtual void OnFunctioningChange(bool functioning)
    {

    }
    public bool HasNeighbor(string name)
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
    public int CountInRange(string name, float range)//name为null就是所有建筑都统计
    {
        Vector2Int u, v;
        int t = Mathf.CeilToInt(range);
        HashSet<BaseBuilding> set = new();
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                for (int ii = -t; ii <= t; ++ii)
                {
                    for (int jj = -t; jj <= t; ++jj)
                    {
                        if (ii*ii+jj*jj<=range*range)
                        {
                            u = v + new Vector2Int(ii, jj);
                            if (register.ContainsKey(u) && register[u]!= null && (name == null || register[u].name==name))
                                set.Add(register[u]);
                        }
                    }
                }
            }
        }
        return set.Count;
    }
    public HashSet<BaseBuilding> GetNeighborsInRange(float range)
    {
        Vector2Int u, v;
        int t = Mathf.CeilToInt(range);
        HashSet<BaseBuilding> set = new();
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                for (int ii = -t; ii <= t; ++ii)
                {
                    for (int jj = -t; jj <= t; ++jj)
                    {
                        if (ii * ii + jj * jj <= range * range)
                        {
                            u = v + new Vector2Int(ii, jj);
                            if (register.ContainsKey(u) && register[u] != null)
                                set.Add(register[u]);
                        }
                    }
                }
            }
        }
        return set;
    }
    public virtual void ReportUpgrade()
    {
        //所有建筑升级时都还得通知一下，需要记录各种建筑的最高等级
        var hl = BuildingManager.Instance.highestLevelBuilding;
        if (!hl.ContainsKey(name))
        {
            hl[name] = this;
        }
        else
        {
            hl[name] = hl[name].level > level ? hl[name]:this;
        }
    }

    //决定弹出面板的升级按钮是否置灰
    public bool CanUpgrade()
    {
        if(level == buildingInfoPro.buildingInfo.maxLevel)
        {
            return false;
        }
        foreach (KeyValuePair<string, int> pair in buildingInfoPro.upgradeRestrictionList[level])
        {
            if (BuildingManager.Instance.highestLevelBuilding[pair.Key].level < pair.Value)
            {
                return false;
            }
        }
        if (!BuildingManager.Instance.freeDict.ContainsKey(name) || !BuildingManager.Instance.freeDict[name])
        {
            foreach (var t in buildingInfoPro.costList[0])
            {
                if (ResourceManager.Instance.GetResourceCount(t.Key) < t.Value)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void StartStrike(float time)
    {
        onStrike = true;
        strikeTimeLeft = time;
        stationedCountBeforeStrike = stationedCount;
        ManuallyAdjustStation(-stationedCount);
    }
    public bool IsVisible()
    {
        Vector2Int v;
        for (int i = 0; i < span.x; ++i)
        {
            for (int j = 0; j < span.y; ++j)
            {
                v = position + new Vector2Int(i, j);
                if (LandscapeManager.Instance.visibilityMap[v])
                {
                    return true;
                }
            }
        }
        return false;
    }
}
