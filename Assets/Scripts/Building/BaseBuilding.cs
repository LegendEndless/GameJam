using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    new public string name;
    public int level = 0;
    public Vector2Int position;
    public Vector2Int span;
    public int stationedCount;//ע��д���⽨��Ч���߼�ʱҲҪ������û��������פһ��

    public bool upgrading = false;
    //д�������������������������Ȼ��ɻ��ν������Ļ���ܷ���
    public float timeSinceUpgrade;
    public float currentUpgradeDuration;

    public BuildingInfoPro buildingInfoPro;
    public Dictionary<Vector2Int, BaseBuilding> register;

    
    //�е�����𡣡���������һ�¹ҽű�����ʵ����Ԥ����ʱ����Щ������Ҫ��ʼ��
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

        var cd = BuildingManager.Instance.buildingCountDict;
        if (!cd.ContainsKey(name))
            cd[name] = 1;
        else
            ++cd[name];

        buildingInfoPro = BuildingManager.Instance.buildingInfoDict[name];

        level = 0;

        stationedCount = 0;
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
    public virtual void Update()
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
        RealTimeBuilder.Instance.Demolish(position);
        Destroy(gameObject);
    }
    public void PopUI()
    {

    }

    //�����������������UI����פ����
    public int StationedMax()
    {
        return Mathf.Min((level + 5), Mathf.FloorToInt(ResourceManager.Instance.GetResourceCount("PeopleAvailable")));
    }

    public virtual void AutoAdjustStation()
    {
        if(stationedCount == 0 && ResourceManager.Instance.GetResourceCount("PeopleAvailable") <=0)
        {
            //û����פ��ɶҲ����
            return;
        }
        //������Դ������Ĭ����1��
        ManuallyAdjustStation(1-stationedCount);
    }
    public virtual void ManuallyAdjustStation(int delta)
    {
        if(delta==0)
        {
            return;
        }
        stationedCount += delta;
        ResourceManager.Instance.AddResource("PeopleAvailable", -delta);
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
    public int CountInRange(string name, float range)
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
                            if (register.ContainsKey(u) && register[u]!= null && register[u].name==name)
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
        //���н�������ʱ������֪ͨһ�£���Ҫ��¼���ֽ�������ߵȼ�
        var hl = BuildingManager.Instance.highestLevel;
        if (!hl.ContainsKey(name))
        {
            hl[name] = level;
        }
        else
        {
            hl[name] = Mathf.Max(hl[name], level);
        }
    }
}
