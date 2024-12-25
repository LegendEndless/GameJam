using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResourceDisplay : MonoBehaviour
{
    public static ResourceDisplay Instance;

    [System.Serializable]
    public class ResourceText
    {
        public string resourceName;
        public Text textComponent;
    }

    public List<ResourceText> resourceTexts;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateAllResourceTexts();
    }

    public void UpdateAllResourceTexts()
    {
        foreach (var resourceText in resourceTexts)
        {
            UpdateResourceText(resourceText.resourceName);
        }
    }

    public void UpdateResourceText(string resourceName)
    {
        ResourceText resourceText = resourceTexts.Find(rt => rt.resourceName == resourceName);
        if (resourceText != null && ResourceManager.Instance != null)
        {
            float value = ResourceManager.Instance.GetResourceCount(resourceName);
            resourceText.textComponent.text = FormatResourceValue(value);
        }
    }

 
    private string FormatResourceValue(float value)
    {
        if (value < 100)
        {
            return value.ToString("F0"); // ����С�� 100 ��ֵ����ʾΪ����
        }
        else
        {
            return (value / 1000f).ToString("F1") + "k";
        }
    }
}