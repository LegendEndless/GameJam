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
        populationText.text = "��ǰ�˿ڣ�" + Mathf.FloorToInt(currentPopulation).ToString();
    }
}