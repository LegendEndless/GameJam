using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel Instance { get; private set; }
    public Text text1, text2, text3;
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    public void Show(string name)
    {
        text1.text = BuildingManager.Instance.buildingInfoDict[name].buildingInfo.nameChinese;
        text2.text = BuildingManager.Instance.buildingInfoDict[name].buildingInfo.description;
        string str = "���컨��:\n";
        foreach (var pair in BuildingManager.Instance.buildingInfoDict[name].costList[0])
        {
            str += BuildingInfoManager.Instance.dic[pair.Key] + "*" + pair.Value.ToString() + " ";
        }
        if (BuildingManager.Instance.buildingInfoDict[name].upgradeRestrictionList[0].Count > 0)
        {
            str += "\n��������:";
            foreach (var pair in BuildingManager.Instance.buildingInfoDict[name].upgradeRestrictionList[0])
            {
                str += "\n��Ҫ��" + BuildingManager.Instance.buildingInfoDict[pair.Key].buildingInfo.nameChinese + "�ﵽ" + pair.Value + "��";
            }
        }
        text3.text = str;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
