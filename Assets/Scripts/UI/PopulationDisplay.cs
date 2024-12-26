using UnityEngine;
using UnityEngine.UI;

public class PopulationDisplay : MonoBehaviour
{
    public Text populationText; // 连接到UI中的Text组件

    void Update()
    {
        // 获取当前人口
        float currentPopulation = PopulationManager.Instance.currentPopulation;

        // 更新UI显示
        populationText.text = "当前人口：" + Mathf.FloorToInt(currentPopulation).ToString();
    }
}