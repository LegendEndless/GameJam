using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LandscapeManager : MonoBehaviour
{
    //用1,2,4,8,16,32表示各种地形
    public Dictionary<Vector2Int, int> landscapeMap;
    public Dictionary<Vector2Int, bool> visibleMap;
    public Dictionary<Vector2Int, bool> buildableMap;
    public HashSet<Vector2Int> magnetics;
    public HashSet<Vector2Int> airTowers;
    public static LandscapeManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
        landscapeMap = new Dictionary<Vector2Int, int>();
        visibleMap = new Dictionary<Vector2Int, bool>();
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                visibleMap[new Vector2Int(i, j)] = false;
            }
        }
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                visibleMap[new Vector2Int(i, j)] = true;
            }
        }
        buildableMap = new Dictionary<Vector2Int, bool>();
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                buildableMap[new Vector2Int(i, j)] = false;
            }
        }
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                buildableMap[new Vector2Int(i, j)] = true;
            }
        }
        magnetics = new HashSet<Vector2Int>();
        airTowers = new HashSet<Vector2Int>();
        //测试！暂时这么写
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                landscapeMap[new Vector2Int(i, j)] = 1<<0;
            }
        }
    }
    private void Start()
    {
        
    }
    public void RecalculateVisibility()
    {
        float range = 3.17f;//硬编码
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                visibleMap[new Vector2Int(i, j)] = false;
            }
        }
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                visibleMap[new Vector2Int(i,j)] = true;
            }
        }
        foreach(Vector2Int v in magnetics)
        {
            int t = Mathf.CeilToInt(range);
            for (int ii = -t; ii <= t; ++ii)
            {
                for (int jj = -t; jj <= t; ++jj)
                {
                    if (ii * ii + jj * jj <= range * range)
                    {
                        visibleMap[v + new Vector2Int(ii, jj)] = true;
                    }
                }
            }
        }
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                RealTimeBuilder.Instance.tilemap2.SetTile(new Vector3Int(i, j, 1), visibleMap[new Vector2Int(i, j)] ? null : RealTimeBuilder.Instance.mistTile);
            }
        }
    }
    public void RecalculateBuildability()
    {
        float range = 2.24f;//硬编码
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                buildableMap[new Vector2Int(i, j)] = false;
            }
        }
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                buildableMap[new Vector2Int(i, j)] = true;
            }
        }
        foreach (Vector2Int v in airTowers)
        {
            int t = Mathf.CeilToInt(range);
            for (int ii = -t; ii <= t; ++ii)
            {
                for (int jj = -t; jj <= t; ++jj)
                {
                    if (ii * ii + jj * jj <= range * range)
                    {
                        buildableMap[v + new Vector2Int(ii, jj)] = true;
                    }
                }
            }
        }
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                RealTimeBuilder.Instance.tilemap2.SetTile(new Vector3Int(i, j, 0), buildableMap[new Vector2Int(i, j)] ? null : RealTimeBuilder.Instance.unbuildableTile);
            }
        }
    }
}
