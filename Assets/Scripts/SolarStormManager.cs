using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SolarStormInfo
{
    public float second;
    public float foodDemand;
    public float waterDemand;
    public float mineDemand;
    public float oilDemand;
    public float electricDemand;
    public float carbonDemand;
    public float tiDemand;
    public float chipDemand;
}
public class SolarStormInfoCollection
{
    public List<SolarStormInfo> infos;
    //public Text timeLeftText; // ����UI

}
public class SolarStormManager : MonoBehaviour
{
    public float time;
    public float unscaledTime;
    public float stormDuration = 20;
    public float warningDuration = 180;
    public float stormStart;
    public bool inStorm;

    public SolarStormInfo info;
    SolarStormInfoCollection collection;
    int index;
    public static SolarStormManager Instance
    {
        get; private set;
    }
    public float TimeLeft//��Ԥ��UI
    {
        get
        {
            return stormStart - time;
        }
    }
    private void Awake()
    {
        Instance = this;
        collection = XmlDataManager.Instance.Load<SolarStormInfoCollection>("solar");
        index = 0;
        stormStart = collection.infos[index].second;
        inStorm = false;
    }
    
    public void StartSolarStorm()
    {
        Time.timeScale = 0;
        inStorm = true;
        info = collection.infos[index];
    }
    public bool CheckGameOver()
    {
        if (info.foodDemand > ResourceManager.Instance.GetResourceCount("food")
        || info.waterDemand > ResourceManager.Instance.GetResourceCount("water")
        || info.mineDemand > ResourceManager.Instance.GetResourceCount("mine")
        || info.oilDemand > ResourceManager.Instance.GetResourceCount("oil")
        || info.electricDemand > ResourceManager.Instance.GetResourceCount("electric")
        || info.carbonDemand > ResourceManager.Instance.GetResourceCount("carbon")
        || info.tiDemand > ResourceManager.Instance.GetResourceCount("ti")
        || info.chipDemand > ResourceManager.Instance.GetResourceCount("chip"))
        {
            //��һ��UI��ʾ���û�ܳŹ��籩����ȷ��������Scene��
            return true;
        }
        return false;
    }

    void Update()
    {
        if (!inStorm)
        {
            time += Time.deltaTime;

            //timeLeftText.text = "Time Left: " + TimeLeft.ToString("F2"); // ����UI�ı�
            if (time > stormStart)
            {
                if(CheckGameOver()) return;
                StartSolarStorm();
            }
        }
        else
        {
            unscaledTime += Time.unscaledDeltaTime;
            if (unscaledTime > stormDuration)
            {
                unscaledTime = 0;
                inStorm = false; 
                //���¼� �¼�ѡ�Ĭ�ϰ�һ���ָ�timeScale
            }
            ResourceManager.Instance.AddResource("food", -info.foodDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("water", -info.waterDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("mine", -info.mineDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("oil", -info.oilDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("electric", -info.electricDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("carbon", -info.carbonDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("ti", -info.tiDemand / stormDuration * Time.unscaledDeltaTime);
            ResourceManager.Instance.AddResource("chip", -info.chipDemand / stormDuration * Time.unscaledDeltaTime);
        }
    }
}