using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuildingMenuController : MonoBehaviour
{
    public GameObject menuPanel; // 菜单界面
    public Button buttonBuild; // 主按钮
    public Button[] categoryButtons; // 分类按钮
    public GameObject[] buildingCategories; // 建筑类别
    public RealTimeBuilder realTimeBuilder; // 引用RealTimeBuilder

    public GameObject launchPanel; // 发射菜单
    public Button buttonLaunch; // 发射按钮//其实是星舰面板按钮哈

    private bool isMenuVisible = false; // 建筑菜单是否可见
    private bool isLaunchVisible = false; // 发射菜单是否可见

    public Text t1, t2, t3, t4;

    public List<bool> interactable;

    public Button buttonWin;

    void Start()
    {
        interactable = new List<bool>();
        for (int i = 0; i < 30; ++i)
        {
            interactable.Add(true);
        }

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
        foreach (var category in buildingCategories)
        {
            for (int i = 0; i < category.transform.childCount; i++)
            {
                GameObject go = category.transform.GetChild(i).gameObject;
                int index2 = i;
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!interactable[index2]) return;
                    RealTimeBuilder.Instance.Select(go.name);
                });
                EventTrigger trigger = go.AddComponent<EventTrigger>();

                EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
                pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
                pointerEnterEntry.callback.AddListener((data) =>
                {
                    InfoPanel.Instance.Show(go.name);
                });
                trigger.triggers.Add(pointerEnterEntry);

                EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
                pointerExitEntry.eventID = EventTriggerType.PointerExit;
                pointerExitEntry.callback.AddListener((data) =>
                {
                    InfoPanel.Instance.Hide();
                });
                trigger.triggers.Add(pointerExitEntry);
            }
        }
        buttonWin.onClick.AddListener(() =>
        {
            if (ResourceManager.Instance.GetResourceCount("life_part") >= 10
            && ResourceManager.Instance.GetResourceCount("chip_part") >= 2
            && ResourceManager.Instance.GetResourceCount("nuclear_part") >= 5
            && ResourceManager.Instance.GetResourceCount("shell_part") >= 5)
            {
                SceneManager.LoadScene("GameOverScene");
            }
        });
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
        if (isLaunchVisible)
        {
            t1.text = ResourceManager.Instance.GetResourceCount("life_part").ToString();
            t2.text = ResourceManager.Instance.GetResourceCount("chip_part").ToString();
            t3.text = ResourceManager.Instance.GetResourceCount("nuclear_part").ToString();
            t4.text = ResourceManager.Instance.GetResourceCount("shell_part").ToString();
        }
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
        foreach (var category in buildingCategories)
        {
            if (category.activeInHierarchy)
            {
                for (int i = 0; i < category.transform.childCount; i++)
                {
                    interactable[i] = RealTimeBuilder.Instance.CanSelect(category.transform.GetChild(i).gameObject.name);
                    category.transform.GetChild(i).gameObject.GetComponent<Image>().color = interactable[i] ? Color.white : Color.grey;
                }
            }
        }
    }
}