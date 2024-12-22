using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��дһ��buildingInfo����Ϊ�˶���ÿ�ֽ�����ֻ��ȡһ�������ļ�
public class BuildingInfo
{
    public string icon;
    public int type;
    public string restrictionString;
    public int restriction;//��Ҫ�ֶ������
    public int extraRestrictionId;
    public SerializableDictionary<string, float> neiborBonusDict;
    public int sizeX;
    public int sizeY;
    public float massProduction;
    public float singleProduction;
    public float disasterBoost;
    public int maxCount;
    public float buffSize;
    public int maxLevel;
    public List<SerializableDictionary<string, int>> UpgradeCost;
    public List<float> upgradeDuration;
    public float stationBonus;
    public string description;
}
