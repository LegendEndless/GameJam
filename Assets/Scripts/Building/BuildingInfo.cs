using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//另写一个buildingInfo类是为了对于每种建筑都只读取一次配置文件
public class BuildingInfo
{
    public string name;
    public string icon;
    public int type;
    public string restriction;
    public int extraRestrictionId;
    public string neighborBonus;
    public int sizeX;
    public int sizeY;
    public string production;
    public float disasterBoost;
    public int maxCount;
    public float buffSize;
    public int maxLevel;
    public string upgradeCost;
    public string upgradeDuration;
    public float stationBonus;
    public string description;
}
public class BuildingInfoPro
{
    public BuildingInfo buildingInfo;
    public int restrictionMask;
    public Dictionary<string, float> neighborBonusDict;
    public List<Dictionary<string, float>> productionList;
    public List<Dictionary<string, float>> costList;
    public List<float> durationList;
    public BuildingInfoPro(BuildingInfo info)
    {
        buildingInfo = info;
        string[] parts = buildingInfo.restriction.Split(';');
        restrictionMask = 0;
        foreach (string part in parts)
        {
            restrictionMask |= 1 << (int.Parse(part) - 1);
        }
        neighborBonusDict = new Dictionary<string, float>();
        parts = buildingInfo.neighborBonus.Split(",");
        foreach (string part in parts)
        {
            string[] element = part.Split("|");
            neighborBonusDict[element[0]] = float.Parse(element[1]);
        }
        productionList = new List<Dictionary<string, float>>();
        parts = buildingInfo.production.Split(";");
        foreach (string part in parts)
        {
            Dictionary<string, float> dic = new Dictionary<string, float>();
            string[] pairs = part.Split(",");
            foreach(string pair in pairs)
            {
                string[] element = pair.Split("|");
                dic[element[0]]=float.Parse(element[1]);
            }
            productionList.Add(dic);
        }
        costList = new List<Dictionary<string, float>>();
        parts = buildingInfo.upgradeCost.Split(";");
        foreach (string part in parts)
        {
            Dictionary<string, float> dic = new Dictionary<string, float>();
            string[] pairs = part.Split(",");
            foreach (string pair in pairs)
            {
                string[] element = pair.Split("|");
                dic[element[0]] = float.Parse(element[1]);
            }
            costList.Add(dic);
        }
        durationList = new List<float>();
        parts = buildingInfo.upgradeDuration.Split(";");
        foreach (string part in parts)
        {
            durationList.Add(float.Parse(part));
        }
    }
}
//我之前写的那个XmlDataManager不这样封装个一层就用不了...
public class BuildingInfoCollection
{
    public List<BuildingInfo> buildingInfos;
}
