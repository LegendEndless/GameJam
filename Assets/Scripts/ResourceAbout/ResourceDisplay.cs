using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    public static ResourceDisplay Instance;

    public Text electricText;
    public Text mineText;
    public Text foodText;
    public Text waterText;
    public Text oilText;
    public Text chipText;
    public Text tiText;
    public Text carbonText;

    //显示时间
    //public Text timeLeftText; 

    void Start()
    {
        RegisterResourceText("electric", electricText);
        RegisterResourceText("mine", mineText);
        RegisterResourceText("food", foodText);
        RegisterResourceText("water", waterText);
        RegisterResourceText("oil", oilText);
        RegisterResourceText("chip", chipText);
        RegisterResourceText("ti", tiText);
        RegisterResourceText("carbon", carbonText);

        UpdateAllResourceTexts();
    }

    public Dictionary<string, Text> resourceTexts = new Dictionary<string, Text>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterResourceText(string resourceName, Text textComponent)
    {
        if (!resourceTexts.ContainsKey(resourceName))
        {
            resourceTexts.Add(resourceName, textComponent);
        }
    }

    public void UpdateResourceText(string resourceName)
    {
        if (resourceTexts.ContainsKey(resourceName) && ResourceManager.Instance != null)
        {
            float resourceAmount = ResourceManager.Instance.GetResourceCount(resourceName);
            resourceTexts[resourceName].text = FormatResourceValue(resourceAmount);
        }
    }
    private string FormatResourceValue(float value)
    {
        if (value < 1000)
        {

            return value.ToString("F0");
        }
        else if (value >= 1000 && value < 10000)
        {

            return (value / 1000f).ToString("F1") + "k";
        }
        else
        {

            return (value / 10000f).ToString("F1") + "w";
        }
    }
    public void UpdateAllResourceTexts()
    {
        foreach (var resourcePair in resourceTexts)
        {
            UpdateResourceText(resourcePair.Key);
            // UpdateTimeText(); // 调用更新时间的方法
        }
    }
    private void Update()
    {
        UpdateAllResourceTexts();
    }

    public void Pause()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        else Time.timeScale = 0;
    }
    public void Accelerate()
    {
        Time.timeScale = 10;
    }

    //更新时间的方法（不知道能不能这么写）
    //private void UpdateTimeText()
    //{
    //    if (SolarStormManager.Instance != null)
    //   {

    //       float timeRemaining = SolarStormManager.Instance.TimeLeft; // 剩余时间
    //       Debug.Log("时间剩余 " + timeRemaining); // 调试
    //       if (timeRemaining < 0) timeRemaining = 0; 

    //       int minutes = Mathf.FloorToInt(timeRemaining / 60);
    //       int seconds = Mathf.FloorToInt(timeRemaining % 60);

    //       string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

    //       if (timeLeftText != null)
    //      {
    //          timeLeftText.text = formattedTime;
    //      }
    //      else
    //      {

    //      }

    // }
    //  else
    //  {
    //     Debug.LogError("没有SolarStormManager");
    //     if (timeLeftText != null)
    //      {
    //         timeLeftText.text = "00:00"; // 默认值
    //     }
    // }
    // }
}