public class LAN : ProductionBuilding
{
    public override void OnFunctioningChange(bool functioning)
    {
        base.OnFunctioningChange(functioning);
        int change = functioning ? 1 : -1;
        var set = GetNeighborsInRange(4.01f);//Ӳ����
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
            //û����פ��ɶҲ����
            return;
        }
        //������Դ������Ĭ����1��
        ManuallyAdjustStation(1 - stationedCount);
    }
}
