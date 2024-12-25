using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public PlotEventCollection plotEvents;
    public RegularEventCollection disasterEvents, socialEvents;
    float time;
    float cdDisaster;
    float cdSocial;
    int plotIndex;
    RegularEventInfo disasterInfo, socialInfo;
    public static EventManager Instance { get; private set; }

    //引用EventUIManager 1224晚修改





    private void Awake()
    {
        Instance = this;
        plotEvents = XmlDataManager.Instance.Load<PlotEventCollection>("plot");
        disasterEvents = XmlDataManager.Instance.Load<RegularEventCollection>("disaster");
        socialEvents = XmlDataManager.Instance.Load<RegularEventCollection>("social");
        time = 0;
        plotIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (plotIndex > plotEvents.events.Count || time > plotEvents.events[plotIndex].occurrenceTime)
        {
            PopPlotUI(plotEvents.events[plotIndex]);
            ++plotIndex;
        }
        cdDisaster -= (1 - LivabilityManager.Instance.livability / 40) * Time.deltaTime;
        if (cdDisaster < 0)
        {
            disasterInfo = disasterEvents.events[Random.Range(0, disasterEvents.events.Count)];
            PopRegularUI(disasterInfo);
            cdDisaster = 180;
            disasterEvents.events.Remove(disasterInfo);
            if (disasterEvents.events.Count == 0)
            {
                disasterEvents = XmlDataManager.Instance.Load<RegularEventCollection>("disaster");
            }
        }
        cdSocial -= Time.deltaTime;
        if (cdSocial < 0)
        {
            socialInfo = socialEvents.events[Random.Range(0, socialEvents.events.Count)];
            PopRegularUI(socialInfo);
            cdSocial = 180 + Random.Range(-30f, 30f);
            socialEvents.events.Remove(socialInfo);
            if (socialEvents.events.Count == 0)
            {
                socialEvents = XmlDataManager.Instance.Load<RegularEventCollection>("social");
            }
        }
    }
    public void PopPlotUI(PlotEventInfo info)
    {
        EventUIManager.Instance.ShowPlotEvent(info);
    }
    public void PopRegularUI(RegularEventInfo info)
    {
        //每次弹出都得重新设置所有选项能否选择 用CanChoose()
        EventUIManager.Instance.ShowRegularEvent(info);

    }
    public List<UnityAction> TranslateEffect(string effect)
    {
        List<UnityAction> list = new List<UnityAction>();
        string[] singleEffects = effect.Split(';');
        foreach (string singleEffect in singleEffects)
        {
            string[] tmp = singleEffect.Split(':');
            switch (tmp[0])
            {
                case "A":
                    list.Add(() => AddResource(tmp[1]));
                    break;
                case "B":
                    list.Add(() =>
                    {
                        LivabilityManager.Instance.eventLivability += int.Parse(tmp[1]);
                        LivabilityManager.Instance.Recalculate();
                    });
                    break;
                case "C":
                    list.Add(() =>
                    {
                        PopulationManager.Instance.rate += float.Parse(tmp[1]);
                    });
                    break;
                case "D":
                    string[] element = tmp[1].Split('|');
                    list.Add(() =>
                    {
                        if (!BuildingManager.Instance.highestLevelBuilding.ContainsKey(element[0])) return;
                        if (BuildingManager.Instance.highestLevelBuilding[element[0]] == null)
                        {
                            BuildingManager.Instance.UpdateHighestLevel(element[0]);
                        }
                        if (BuildingManager.Instance.highestLevelBuilding[element[0]] != null)
                            BuildingManager.Instance.highestLevelBuilding[element[0]].StartStrike(float.Parse(element[1]));
                    });
                    break;
                case "E":
                    list.Add(() =>
                    {
                        BuildingManager.Instance.freeDict[tmp[1]] = true;
                    });
                    break;
            }
        }
        return list;
    }
    public bool CanChoose(string effect)
    {
        string[] singleEffects = effect.Split(';');
        foreach (string singleEffect in singleEffects)
        {
            string[] tmp = singleEffect.Split(':');
            if (tmp[0] == "A" && !CanAddResource(tmp[1]))
            {
                return false;
            }
        }
        return true;
    }
    public bool CanAddResource(string effect)
    {
        string[] pairs = effect.Split(',');
        foreach (string pair in pairs)
        {
            string[] element = pair.Split('|');
            if (element[0] != "" && ResourceManager.Instance.resources[element[0]] + float.Parse(element[1]) < 0)
            {
                return false;
            }
        }
        return true;
    }
    public void AddResource(string effect)
    {
        string[] pairs = effect.Split(',');
        foreach (string pair in pairs)
        {
            string[] element = pair.Split('|');
            if (element[0] != "")
            {
                ResourceManager.Instance.AddResource(element[0], float.Parse(element[1]));
            }
        }
    }
}

