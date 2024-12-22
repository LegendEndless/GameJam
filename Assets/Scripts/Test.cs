using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BuildingInfo testInfo = new BuildingInfo
        {
            name = "ss",
            icon = "path/to/icon",
            type = 1,
            restriction = "1,2,3,4,5,6",
            extraRestrictionId = 0,
            neighborBonus = "neighbor1|0.1,neighbor2|0.2",
            sizeX = 2,
            sizeY = 2,
            production = "100,120,145,175,210",
            disasterBoost = 1,
            maxCount = 3,
            buffSize = 1.414f,
            maxLevel = 5,
            upgradeCost = "food|1,water|1;food|1,water|2;food|3,water|3,oil|1",
            upgradeDuration = "10,15,20,25,30",
            stationBonus = 5,
            description = "Õâ¸ö½¨Öþblabla",
        };
        BuildingInfoCollection collection = XmlDataManager.Instance.Load<BuildingInfoCollection>("building1");
        print(collection.buildingInfos[1].upgradeCost);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}