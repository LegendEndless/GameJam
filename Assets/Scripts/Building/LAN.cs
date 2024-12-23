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
            //通知范围内的生产建筑，能影响到它们的基站变化了一个
            int change = functioning ? 1 : -1;
            var set = GetNeighborsInRange(2.01f);//硬编码
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
            //没人派驻，啥也不做
            return;
        }
        //不是资源建筑就默认上1吧
        ManuallyAdjustStation(1 - stationedCount);
    }
}
