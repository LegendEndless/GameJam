using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float stormDuration = 5;//
    public float warningDuration = 180;
    public float stormStart;
    public bool isInStorm;
    public Image image;

    public SolarStormInfo info;
    SolarStormInfoCollection collection;
    int index;
    bool isGameOver;
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
        isInStorm = false;
        isGameOver = false;
    }

    public void StartSolarStorm()
    {
        image.gameObject.SetActive(true);
        Time.timeScale = 0;
        isInStorm = true;
        MusicManager.Instance.PlaySolarStormMusic();
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
            EventUIManager.Instance.choiceButton3.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MainMenu");
            });
            EventUIManager.Instance.ShowPlotEvent(new PlotEventInfo
            {
                title = "��������",
                description = "����û��׼����Ӧ��̫���籩�����ʣ��ݵ���ǿ�ҵķ����б��ݻٴ��������࣬������޾��������վ���������Ϊ����",
                option = "�������ǡ���һ��ج�Σ�"
            });
            return true;
        }
        return false;
    }

    void Update()
    {
        if (isGameOver) return;
        if (!isInStorm)
        {
            time += Time.deltaTime;

            //timeLeftText.text = "Time Left: " + TimeLeft.ToString("F2"); // ����UI�ı�
            if (time > stormStart)
            {
                info = collection.infos[index];
                if (CheckGameOver())
                {
                    isGameOver = true;
                    return;
                }
                StartSolarStorm();
            }
        }
        else
        {
            Camera.main.transform.position = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), -10);
            unscaledTime += Time.unscaledDeltaTime;
            if (unscaledTime > stormDuration)
            {
                unscaledTime = 0;
                isInStorm = false;
                image.gameObject.SetActive(false);
                MusicManager.Instance.PlayRandomAmbientMusic();
                EventUIManager.Instance.ShowPlotEvent(new PlotEventInfo
                {
                    title = "��ʱ��ȫ",
                    description = "�ɹ��ˣ����ǻ������ˣ����ڿ�����ʱ�������ˣ������´η籩���ή�١����Ǳ��������ǽ������������Զ�������ܳ��װ�������",
                    option = "���ͣ�"
                });
                ++index;
                stormStart = collection.infos[index].second;
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