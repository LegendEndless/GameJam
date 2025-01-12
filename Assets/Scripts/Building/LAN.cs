public class LAN : ProductionBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        int change = functioning ? 1 : -1;
        var set = GetNeighborsInRange(4.01f);//硬编码
        foreach (BaseBuilding b in set)
        {
            if (b is ProductionBuilding)
            {
                (b as ProductionBuilding).ChangeNumLAN(change);
            }
        }
    }
    public override void AutoAdjustStation()
    {
        if (stationedCount == 0 && PopulationManager.Instance.AvailablePopulation <= 0)
        {
            //没人派驻，啥也不做
            return;
        }
        //不是资源建筑就默认上1吧
        ManuallyAdjustStation(1 - stationedCount);
    }
}
