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

    //有点多余吗。。至少提醒一下挂脚本或者实例化预制体时有哪些参数需要初始化
    public void Initialize(string name,Vector2Int position,Vector2Int span)
    {
        this.name = name;
        this.position = position;
        this.span = span;

        Vector2Int v;
        Dictionary<Vector2Int,BaseBuilding> register = BuildingManager.Instance.landUseRegister;
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
    public void FinishUpgrade()
    {
        ++level;
        upgrading = false;
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
        Dictionary<Vector2Int, BaseBuilding> register = BuildingManager.Instance.landUseRegister;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                register[v] = null;
            }
        }
        Destroy(gameObject);
    }
    public void PopUI()
    {

    }
    public virtual void RecalculateProduction()
    {

    }
}
