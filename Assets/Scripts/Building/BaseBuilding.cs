using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour
{
    public string type;
    public int level = 0;
    public Vector2Int position;
    public Vector2Int span;

    public bool upgrading = false;

    //д�������������������������Ȼ��ɻ��ν������Ļ���ܷ���
    public float timeSinceUpgrade;
    public float currentUpgradeDuration;

    //�е�����𡣡���������һ�¹ҽű�����ʵ����Ԥ����ʱ����Щ������Ҫ��ʼ��
    public void Initialize(string type,Vector2Int position,Vector2Int span)
    {
        this.type = type;
        this.position = position;
        this.span = span;

        Vector2Int v;
        Dictionary<Vector2Int,bool> register = BuildingManager.Instance.landUseRegister;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v = position + new Vector2Int(i, j);
                register[v] = true;
            }
        }

        level = 0;
        Upgrade();
    }
    public void Upgrade()
    {
        upgrading=true;
        timeSinceUpgrade = 0;
        currentUpgradeDuration = BuildingManager.Instance.buildingInfoDict[type].buildingInfo.upgradeDuration[level];
    }
    private void Update()
    {
        if (timeSinceUpgrade >= currentUpgradeDuration)
        {
            ++level;
            upgrading = false;
        }
        if (upgrading)
        {
            timeSinceUpgrade += Time.deltaTime;
        }
    }
    public void Demolish()
    {
        Destroy(gameObject);

    }
}
