using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassProductionBuilding : BaseBuilding
{
    public float production;
    public override void RecalculateProduction()
    {
        base.RecalculateProduction();
        production = buildingInfoPro.buildingInfo.production[level-1];//���-1���һ��
        //�ټ����˿ڽ�פ���ڽ�������bonus
        production += buildingInfoPro.buildingInfo.stationBonus * stationedCount;
        
    }
}
