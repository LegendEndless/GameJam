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
        // ���ý���ͼƬ���÷�����ȡͼƬ��
        // buildingImage.sprite = GetBuildingSprite(currentBuilding.name);

        maxPopulationText.text = $"{currentBuilding.StationedMax()}";
        currentPopulationText.text = $" {currentBuilding.stationedCount}";

        // ������Դ������Ϣ���÷�����ȡ��Դ��
        // currentResourceText.text = GetCurrentResourceProduction(currentBuilding);

        if (currentBuilding.level >= currentBuilding.buildingInfoPro.buildingInfo.maxLevel)
        {
            nextLevelResourceText.gameObject.SetActive(false);
            upgradeCostText.gameObject.SetActive(false);
            maxLevelText.gameObject.SetActive(true);
            maxLevelText.text = "�Ѵﵽ��ߵȼ�";
            upgradeButton.interactable = false;
        }
        else
        {
            nextLevelResourceText.gameObject.SetActive(true);
            upgradeCostText.gameObject.SetActive(true);
            maxLevelText.gameObject.SetActive(false);
            // ������һ����Դ������Ϣ����֪���ǲ�������
            // nextLevelResourceText.text = GetNextLevelResourceProduction(currentBuilding);
            // ��������������Դ��Ϣ����֪���ǲ�������
            // upgradeCostText.text = GetUpgradeCost(currentBuilding);
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
        //Destroy(gameObject); // �ر�UI
    }

    // ����ҪһЩ��������ȡ��Ϣ������
    // private string GetCurrentResourceProduction(BaseBuilding building) 
    // private string GetNextLevelResourceProduction(BaseBuilding building)
    // private string GetUpgradeCost(BaseBuilding building) 
    // private Sprite GetBuildingSprite(string buildingName) 
}