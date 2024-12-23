using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    public float multiplier;
    public float environmentMultiplier;
    int numLAN;
    public override void Initialize(string name, Vector2Int position, Vector2Int span)
    {
        base.Initialize(name, position, span);
        multiplier = 0;
        environmentMultiplier = -1;
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
    public void RecalculateMultiplier(bool causedByEnvironment)
    {
        float lastMultiplier = multiplier;
        if (causedByEnvironment || environmentMultiplier == -1)
        {
            environmentMultiplier = 0;
            foreach (KeyValuePair<string, float> pair in buildingInfoPro.neighborBonusDict)
            {
                if (HasNeighbor(pair.Key))
                {
                    environmentMultiplier += pair.Value;
                }
            }
            if (true)//To do:�ǵ����ж�һ���ǲ�����Ҫ����ͨ�������Ľ�������
            {
                if (numLAN > 0)
                {
                    environmentMultiplier += 0.2f;//ĳ��Ӳ���������ֵ
                }
                environmentMultiplier += BuildingManager.Instance.globalMultiplier;
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
            multiplier += environmentMultiplier;
            //����Ƿ������磬�ټ��Ϸ����ӳ�
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
