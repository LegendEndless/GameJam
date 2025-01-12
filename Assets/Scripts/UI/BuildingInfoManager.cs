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
        {"electric", "����"},
        {"mine", "����"},
        {"food", "ʳ��"},
        {"water", "ˮ"},
        {"oil", "ʯ��"},
        {"chip", "����оƬ"},
        {"ti", "�ѺϽ�"},
        {"carbon", "̼��ά"},
        {"nuclear_part", "�����ƽ���"},
        {"life_part", "����֧�ֲ�"},
        {"shell_part", "�������ο�"},
        {"chip_part", "�����ǿ�о"},
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
            productivity.text = "ÿ������ǲһ��,\n����/���ı���" + (f > 0 ? "+" : "") + (f * 100) + "%"
            + "\n��ǰ����: " + ((currentBuilding as ProductionBuilding).multiplier * 100).ToString() + "%";
            currentResourceText.text = "��ǰ��������/������(ÿ��)\n" + GetCurrentResourceProduction(currentBuilding);
            if (currentBuilding.level < currentBuilding.buildingInfoPro.buildingInfo.maxLevel && currentBuilding.name != "NursingHouse")
            {
                currentResourceText.text += "\n��һ��: " + GetNextLevelResourceProduction(currentBuilding);
            }
        }
        else
        {
            productivity.text = "������ǲһ�ˣ���������ʧȥ���á�";
            switch (currentBuilding.name)
            {
                case "NursingHouse":
                    currentResourceText.text += "\n�����������˾Ӷ�";
                    break;
                case "CellRepair":
                    currentResourceText.text = "�����Լӿ��˿�����";
                    break;
            }
        }
        buildingNameText.text = currentBuilding.buildingInfoPro.buildingInfo.nameChinese;
        // ���ý���ͼƬ���÷�����ȡͼƬ��
        buildingImage.sprite = GetBuildingSprite(currentBuilding.name);

        maxPopulationText.text = $"{currentBuilding.level + 5}";
        currentPopulationText.text = $"{currentBuilding.stationedCount}";

        if (currentBuilding.level >= currentBuilding.buildingInfoPro.buildingInfo.maxLevel)
        {
            upgradeCostText.text = "��Ϊ��ߵȼ�";
            upgradeButton.interactable = false;
        }
        else if (currentBuilding.isUpgrading)
        {
            upgradeCostText.text = "������";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeCostText.text = $"��ǰ{currentBuilding.level}����";
            upgradeCostText.text += "��������: " + GetUpgradeCost(currentBuilding);
            foreach (var pair in currentBuilding.buildingInfoPro.upgradeRestrictionList[currentBuilding.level])
            {
                upgradeCostText.text += "\n��Ҫ��" + BuildingManager.Instance.buildingInfoDict[pair.Key].buildingInfo.nameChinese + "�ﵽ" + pair.Value + "��";
            }
            upgradeCostText.text += "\n��ʱ: " + currentBuilding.buildingInfoPro.durationList[currentBuilding.level] + "s";
            upgradeButton.interactable = currentBuilding.CanUpgrade();
        }

        // ���ð�ť������
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
                str += $"ÿ{(1 / pair.Value).ToString()}������һ��" + dic[pair.Key] + " ";
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
                str += $"ÿ{(1 / pair.Value).ToString()}������һ��" + dic[pair.Key] + " ";
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
        //Destroy(gameObject); // �ر�UI
    }
}