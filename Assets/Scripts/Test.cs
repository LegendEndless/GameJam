using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //BuildingInfo testInfo = new BuildingInfo
        //{
        //    name = "ss",
        //    icon = "path/to/icon",
        //    type = 1,
        //    restriction = "1,2,3,4,5,6",
        //    extraRestrictionId = 0,
        //    neighborBonus = "neighbor1|0.1,neighbor2|0.2",
        //    sizeX = 2,
        //    sizeY = 2,
        //    massProduction = "100,120,145,175,210",
        //    disasterBoost = 1,
        //    maxCount = 3,
        //    buffSize = "1;1.5;2;2.5;3",
        //    maxLevel = 5,
        //    upgradeCost = "food|1,water|1;food|1,water|2;food|3,water|3,oil|1",
        //    upgradeDuration = "10,15,20,25,30",
        //    stationBonus = 5,
        //    description = "Õâ¸ö½¨Öþblabla",
        //};
        //BuildingInfoCollection collection = XmlDataManager.Instance.Load<BuildingInfoCollection>("building1");
        //print(collection.buildingInfos[1].upgradeCost);
        string str = ";;;;";
        List<Dictionary<string,float>> list = new List<Dictionary<string, float>>();
        string[] parts = str.Split(";");
        foreach (string part in parts)
        {
            Dictionary<string, float> dic = new Dictionary<string, float>();
            string[] pairs = part.Split(",");
            foreach (string pair in pairs)
            {
                string[] element = pair.Split("|");
                if (element[0]!="")
                    dic[element[0]] = float.Parse(element[1]);
            }
            list.Add(dic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}