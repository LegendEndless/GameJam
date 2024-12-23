using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour
{
    //��1,2,4,8,16,32��ʾ���ֵ���
    public Dictionary<Vector2Int, int> landscapeMap;
    public static LandscapeManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        landscapeMap = new Dictionary<Vector2Int, int>();
    }
}
