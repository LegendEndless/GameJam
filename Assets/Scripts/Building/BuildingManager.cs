using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    static BuildingManager instance;
    public static BuildingManager Instance=>instance;
    public SerializableDictionary<string, BuildingInfo> buildingInfoDict;
    public Dictionary<string, int> buildingCountDict;
    //����д����ȷ����ͼ��С��Ҳ�ܽ������ε�ͼ
    public Dictionary<Vector2Int, bool> landUseRegister;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //��ȡ�������ñ�buildingInfoDict
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
