using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAN : ProductionBuilding
{
    bool functioning;
    public override void ManuallyAdjustStation(int delta)
    {
        base.ManuallyAdjustStation(delta);
        if(functioning != (stationedCount != 0))
        {
            functioning = (stationedCount != 0);
            //֪ͨ��Χ�ڵ�������������Ӱ�쵽���ǵĻ�վ�仯��һ��
            int change = functioning ? 1 : -1;
            var set = GetNeighborsInRange(2.01f);//Ӳ����
            foreach (BaseBuilding b in set)
            {
                if(b is ProductionBuilding)
                {
                    (b as ProductionBuilding).ChangeNumLAN(change);
                }
            }
        }
    }
    public override void AutoAdjustStation()
    {
        if (stationedCount == 0 && ResourceManager.Instance.GetResourceCount("PeopleAvailable") <= 0)
        {
            //û����פ��ɶҲ����
            return;
        }
        //������Դ������Ĭ����1��
        ManuallyAdjustStation(1 - stationedCount);
    }
}
