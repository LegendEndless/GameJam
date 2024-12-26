using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuController : MonoBehaviour
{
    public GameObject menuPanel; // 菜单界面
    public Button buttonBuild; // 主按钮
    public Button[] categoryButtons; // 分类按钮
    public GameObject[] buildingCategories; // 建筑类别
    public RealTimeBuilder realTimeBuilder; // 引用RealTimeBuilder

    public GameObject launchPanel; // 发射菜单
    public Button buttonLaunch; // 发射按钮

    private bool isMenuVisible = false; // 建筑菜单是否可见
    private bool isLaunchVisible = false; // 发射菜单是否可见

    void Start()
    {
        // 初始化菜单为隐藏状态
        menuPanel.SetActive(false);
        launchPanel.SetActive(false);


        // 绑定主按钮点击事件
        buttonBuild.onClick.AddListener(ToggleMenu);
        // 绑定发射按钮点击事件
        buttonLaunch.onClick.AddListener(ToggleLaunchMenu);

        // 绑定分类按钮点击事件
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i; // 避免闭包问题
            categoryButtons[i].onClick.AddListener(() => ShowCategory(index));
        }
    }

    void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        menuPanel.SetActive(isMenuVisible);
    }
    void ToggleLaunchMenu()
    {
        isLaunchVisible = !isLaunchVisible;
        launchPanel.SetActive(isLaunchVisible);
    }

    void ShowCategory(int index)
    {
        // 隐藏所有类别
        foreach (var category in buildingCategories)
        {
            category.SetActive(false);
        }

        // 显示选中的类别
        buildingCategories[index].SetActive(true);
    }
    private void Update()
    {
        print(RealTimeBuilder.Instance.CanSelect("Magnetic"));
        foreach (var category in buildingCategories)
        {
            if (category.activeInHierarchy)
            {
                for (int i = 0; i < category.transform.childCount; i++)
                {
                    if(category.transform.GetChild(i).gameObject.activeInHierarchy)
                        category.transform.GetChild(i).GetComponent<Button>().interactable = RealTimeBuilder.Instance.CanSelect(category.transform.GetChild(i).name);
                }
            }
        }
    }
}