using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        return (value / 1000f).ToString("F1") + "k";
    }
    public void UpdateAllResourceTexts()
    {
        foreach (var resourcePair in resourceTexts)
        {
            UpdateResourceText(resourcePair.Key);
        }
    }
}