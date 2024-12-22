using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuController : MonoBehaviour
{
    public GameObject menuPanel; // �˵�����
    public Button buttonBuild; // ����ť
    public Button[] categoryButtons; // ���ఴť
    public GameObject[] buildingCategories; // �������

    private bool isMenuVisible = false; // �˵��Ƿ�ɼ�

    void Start()
    {
        // ��ʼ���˵�Ϊ����״̬
        menuPanel.SetActive(false);

        // ������ť����¼�
        buttonBuild.onClick.AddListener(ToggleMenu);

        // �󶨷��ఴť����¼�
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int index = i; // ����հ�����
            categoryButtons[i].onClick.AddListener(() => ShowCategory(index));
        }
    }

    void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        menuPanel.SetActive(isMenuVisible);
    }

    void ShowCategory(int index)
    {
        // �����������
        foreach (var category in buildingCategories)
        {
            category.SetActive(false);
        }

        // ��ʾѡ�е����
        buildingCategories[index].SetActive(true);
    }
}