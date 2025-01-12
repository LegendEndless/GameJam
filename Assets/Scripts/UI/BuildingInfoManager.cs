using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoManager : MonoBehaviour
{
    public Text buildingNameText;
    public Image buildingImage;
    public Text maxPopulationText;
    public Text currentPopulationText;
    public Button decreasePopulationButton;
    public Button increasePopulationButton;
    public Text upgradeCostText;
    public Text maxLevelText;
    public Button upgradeButton;
    public Button demolishButton;
    public Text productivity;
    public Text currentResourceText;
    public Button buttonClose;

    public Dictionary<string, string> dic = new Dictionary<string, string>
    {
        {"electric", "电力"},
        {"mine", "矿物"},
        {"food", "食物"},
        {"water", "水"},
        {"oil", "石油"},
        {"chip", "纳米芯片"},
        {"ti", "钛合金"},
        {"carbon", "碳纤维"},
        {"nuclear_part", "核能推进器"},
        {"life_part", "生命支持舱"},
        {"shell_part", "辐射屏蔽壳"},
        {"chip_part", "导航智控芯"},
    };

    private BaseBuilding currentBuilding;

    public static BuildingInfoManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        buttonClose.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
    private void Update()
    {
        UpdateUI();
    }

    public void SetBuilding(BaseBuilding building)
    {
        currentBuilding = building;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentBuilding == null) return;

        currentResourceText.text = "";
        if (currentBuilding is ProductionBuilding)
        {
            float f = currentBuilding.buildingInfoPro.buildingInfo.stationBonus;
            productivity.text = "每额外派遣一人,\n生产/消耗倍率" + (f > 0 ? "+" : "") + (f * 100) + "%"
            + "\n当前倍率: " + ((currentBuilding as ProductionBuilding).multiplier * 100).ToString() + "%";
            currentResourceText.text = "当前基础生产/消耗率(每秒)\n" + GetCurrentResourceProduction(currentBuilding);
            if (currentBuilding.level < currentBuilding.buildingInfoPro.buildingInfo.maxLevel && currentBuilding.name != "NursingHouse")
            {
                currentResourceText.text += "\n下一级: " + GetNextLevelResourceProduction(currentBuilding);
            }
        }
        else
        {
            productivity.text = "至少派遣一人，否则建筑将失去作用。";
            switch (currentBuilding.name)
            {
                case "NursingHouse":
                    currentResourceText.text += "\n升级以增加宜居度";
                    break;
                case "CellRepair":
                    currentResourceText.text = "升级以加快人口增长";
                    break;
            }
        }
        buildingNameText.text = currentBuilding.buildingInfoPro.buildingInfo.nameChinese;
        // 设置建筑图片（该方法调取图片池
        buildingImage.sprite = GetBuildingSprite(currentBuilding.name);

        maxPopulationText.text = $"{currentBuilding.level + 5}";
        currentPopulationText.text = $"{currentBuilding.stationedCount}";

        if (currentBuilding.level >= currentBuilding.buildingInfoPro.buildingInfo.maxLevel)
        {
            upgradeCostText.text = "已为最高等级";
            upgradeButton.interactable = false;
        }
        else if (currentBuilding.isUpgrading)
        {
            upgradeCostText.text = "升级中";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeCostText.text = $"当前{currentBuilding.level}级，";
            upgradeCostText.text += "升级消耗: " + GetUpgradeCost(currentBuilding);
            foreach (var pair in currentBuilding.buildingInfoPro.upgradeRestrictionList[currentBuilding.level])
            {
                upgradeCostText.text += "\n需要有" + BuildingManager.Instance.buildingInfoDict[pair.Key].buildingInfo.nameChinese + "达到" + pair.Value + "级";
            }
            upgradeCostText.text += "\n用时: " + currentBuilding.buildingInfoPro.durationList[currentBuilding.level] + "s";
            upgradeButton.interactable = currentBuilding.CanUpgrade();
        }

        // 设置按钮监听器
        increasePopulationButton.onClick.RemoveAllListeners();
        increasePopulationButton.onClick.AddListener(() => AdjustPopulation(1));

        decreasePopulationButton.onClick.RemoveAllListeners();
        decreasePopulationButton.onClick.AddListener(() => AdjustPopulation(-1));

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeBuilding);

        demolishButton.onClick.RemoveAllListeners();
        demolishButton.onClick.AddListener(DemolishBuilding);

        demolishButton.interactable = currentBuilding.name != "StarshipCenter";


        decreasePopulationButton.interactable = !(currentBuilding.stationedCount == 0);
        increasePopulationButton.interactable = !(currentBuilding.stationedCount >= currentBuilding.level + 5);

        gameObject.SetActive(true);
    }

    private string GetUpgradeCost(BaseBuilding currentBuilding)
    {
        string str = "";
        foreach (var pair in currentBuilding.buildingInfoPro.costList[currentBuilding.level])
        {
            str += dic[pair.Key] + "*" + pair.Value.ToString() + " ";
        }
        return str;
    }

    private string GetNextLevelResourceProduction(BaseBuilding currentBuilding)
    {
        string str = "";
        foreach (var pair in currentBuilding.buildingInfoPro.massProductionList[currentBuilding.level])
        {
            str += (pair.Value > 0 ? "+" : "") + pair.Value.ToString() + dic[pair.Key] + " ";
        }
        if (currentBuilding.buildingInfoPro.singleProductionList.Count > currentBuilding.level)
        {
            foreach (var pair in currentBuilding.buildingInfoPro.singleProductionList[currentBuilding.level])
            {
                str += $"每{(1 / pair.Value).ToString()}秒生产一个" + dic[pair.Key] + " ";
            }
        }
        return str;
    }

    private string GetCurrentResourceProduction(BaseBuilding currentBuilding)
    {
        string str = "";
        if (currentBuilding.level == 0) return str;
        foreach (var pair in currentBuilding.buildingInfoPro.massProductionList[currentBuilding.level - 1])
        {
            str += (pair.Value > 0 ? "+" : "") + pair.Value.ToString() + dic[pair.Key] + " ";
        }
        if (currentBuilding.buildingInfoPro.singleProductionList.Count > currentBuilding.level)
        {
            foreach (var pair in currentBuilding.buildingInfoPro.singleProductionList[currentBuilding.level - 1])
            {
                str += $"每{(1 / pair.Value).ToString()}秒生产一个" + dic[pair.Key] + " ";
            }
        }
        return str;
    }

    private Sprite GetBuildingSprite(string name)
    {
        return Resources.Load<Sprite>("Sprites/Buildings/" + name);
    }

    private void AdjustPopulation(int delta)
    {
        currentBuilding.ManuallyAdjustStation(delta);
        UpdateUI();
    }

    private void UpgradeBuilding()
    {
        currentBuilding.StartUpgrade();
        UpdateUI();
    }

    private void DemolishBuilding()
    {
        currentBuilding.Demolish();
        //Destroy(gameObject); // 关闭UI
    }
}