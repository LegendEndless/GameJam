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
    public Text currentResourceText;
    public Text nextLevelResourceText;
    public Text upgradeCostText;
    public Text maxLevelText;
    public Button upgradeButton;
    public Button demolishButton;
    public Button closeButton;

    private BaseBuilding currentBuilding;

    public static BuildingInfoManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    private void Start()
    {
        
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void SetBuilding(BaseBuilding building)
    {
        currentBuilding = building;
        UpdateUI();
    }

    private void UpdateUI()
    {

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseMenu);

        gameObject.SetActive(true);


        if (currentBuilding == null) return;

        buildingNameText.text = currentBuilding.name;
        // 设置建筑图片（该方法调取图片池
        // buildingImage.sprite = GetBuildingSprite(currentBuilding.name);

        maxPopulationText.text = $"{currentBuilding.StationedMax()}";
        currentPopulationText.text = $" {currentBuilding.stationedCount}";

        // 更新资源生产信息（该方法调取资源池
        // currentResourceText.text = GetCurrentResourceProduction(currentBuilding);

        if (currentBuilding.level >= currentBuilding.buildingInfoPro.buildingInfo.maxLevel)
        {
            nextLevelResourceText.gameObject.SetActive(false);
            upgradeCostText.gameObject.SetActive(false);
            maxLevelText.gameObject.SetActive(true);
            maxLevelText.text = "已达到最高等级";
            upgradeButton.interactable = false;
        }
        else
        {
            nextLevelResourceText.gameObject.SetActive(true);
            upgradeCostText.gameObject.SetActive(true);
            maxLevelText.gameObject.SetActive(false);
            // 更新下一级资源生产信息（不知道是不是这样
            // nextLevelResourceText.text = GetNextLevelResourceProduction(currentBuilding);
            // 更新升级所需资源信息（不知道是不是这样
            // upgradeCostText.text = GetUpgradeCost(currentBuilding);
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

        gameObject.SetActive(true);
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

    // 还需要一些方法来获取信息，例如
    // private string GetCurrentResourceProduction(BaseBuilding building) 
    // private string GetNextLevelResourceProduction(BaseBuilding building)
    // private string GetUpgradeCost(BaseBuilding building) 
    // private Sprite GetBuildingSprite(string buildingName) 
}