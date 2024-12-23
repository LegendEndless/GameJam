using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    public float multiplier;
    public float neighborMultiplier;
    int numLAN;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        multiplier = 0;
        neighborMultiplier = -1;
        //�տ�ʼ�͵�ͳ��һ���ܱ߻�վ����
        numLAN = CountInRange("LAN", 2.01f);//Ӳ���봦
    }
    public override void AutoAdjustStation()
    {
        ManuallyAdjustStation(StationedMax() - stationedCount);
    }
    public override void ManuallyAdjustStation(int delta)
    {
        base.ManuallyAdjustStation(delta);
        RecalculateMultiplier(false);
    }
    public void RecalculateMultiplier(bool causedByNeighbor)
    {
        float lastMultiplier = multiplier;
        if (causedByNeighbor || neighborMultiplier == -1)
        {
            neighborMultiplier = 0;
            foreach (KeyValuePair<string, float> pair in buildingInfoPro.neighborBonusDict)
            {
                if (HasNeighbor(pair.Key))
                {
                    neighborMultiplier += pair.Value;
                }
            }
            //��������ٿ�һ����û���Ǹ�ͨ��������
            if (numLAN > 0)//To do:�ǵ����ж�һ���ǲ�����Ҫ����ͨ�������Ľ�������
            {
                neighborMultiplier += 0.2f;//ĳ��Ӳ���������ֵ
            }
        }
        if (stationedCount == 0)
        {
            multiplier = 0;
        }
        else
        {
            multiplier = 1f;
            multiplier += (stationedCount - 1) * buildingInfoPro.buildingInfo.stationBonus;
            multiplier += neighborMultiplier;
        }
        BuildingManager.Instance.ReportMultiplierChange(this,multiplier-lastMultiplier);
    }
    public override void ReportUpgrade()
    {
        base.ReportUpgrade();
        BuildingManager.Instance.ReportUpgrade(this);
    }
    public void ChangeNumLAN(int change)
    {
        numLAN += change;
        RecalculateMultiplier(true);
    }
}
