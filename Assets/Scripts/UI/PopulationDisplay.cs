using UnityEngine;
using UnityEngine.UI;

public class PopulationDisplay : MonoBehaviour
{
    public Text populationText; // ���ӵ�UI�е�Text���

    void Update()
    {
        // ��ȡ��ǰ�˿�
        float currentPopulation = PopulationManager.Instance.currentPopulation;

        // ����UI��ʾ
        populationText.text = "��ǰ�˿ڣ�" + Mathf.FloorToInt(currentPopulation).ToString()
            +"\n�����˿ڣ�" + Mathf.FloorToInt(PopulationManager.Instance.AvailablePopulation)
            +"\n�˿����ޣ�" + PopulationManager.Instance.maxPopulation
            +"\n�˿������ʣ�" + PopulationManager.Instance.rate * 100 + "%";
    }
}