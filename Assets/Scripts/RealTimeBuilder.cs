using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealTimeBuilder : MonoBehaviour
{
    public static RealTimeBuilder Instance
    {
        get; private set;
    }

    public TileBase tile;
    public string buildingName;
    public bool flip;//是否镜像，每次选择新的建筑种类也要重设为false

    public Grid grid;
    public Tilemap tilemap;
    public float padding = 100;

    //public GameObject buildingPrefab;  // 为添加建筑做测试 暂时先用不到

    Vector3Int lastPosition;
    TileBase lastTile;
    Color translucent;
    bool building;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        translucent = new Color(1, 1, 1, 0.8f);
        lastTile = tilemap.GetTile(lastPosition);
    }

    void Update()
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 300 * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel"),2,10);
        if (Input.mousePosition.x < padding)
        {
            Camera.main.transform.Translate(5 * Time.deltaTime * Vector2.left);
        }
        if (Input.mousePosition.x > Screen.width - padding)
        {
            Camera.main.transform.Translate(5 * Time.deltaTime * Vector2.right);
        }
        if (Input.mousePosition.y < padding)
        {
            Camera.main.transform.Translate(5 * Time.deltaTime * Vector2.down);
        }
        if (Input.mousePosition.y > Screen.height - padding)
        {
            Camera.main.transform.Translate(5 * Time.deltaTime * Vector2.up);
        }

        if (building)
        {
            Vector3Int v = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            v.z = 1;
            if (Input.GetKeyDown(KeyCode.F))
            {
                flip = !flip;
                tile = Resources.Load<Tile>("Tiles/" + buildingName + (flip?"_flip":""));
            }
            BuildingInfo info = BuildingManager.Instance.buildingInfoDict[buildingName].buildingInfo;
            Vector2Int span;
            if (!flip)
            {
                span = new Vector2Int(info.sizeX, info.sizeY);
            }
            else
            {
                span = new Vector2Int(info.sizeY, info.sizeX);
            }
            if (lastPosition != v)
            {
                tilemap.SetTile(lastPosition, lastTile);
                tilemap.SetColor(lastPosition,Color.white);
                lastPosition = v;
                lastTile = tilemap.GetTile(lastPosition);
            }
            bool b = CanBuild(buildingName,v,span);
            tilemap.SetTile(v, tile);
            tilemap.RemoveTileFlags(v, TileFlags.LockColor);
            tilemap.SetColor(v, b ? Color.green : Color.red);

            if (Input.GetMouseButtonDown(0) && b)
            {
                lastTile = tile;
                GameObject gameObject = new GameObject(buildingName);
                
                switch (info.type)
                {
                    case 1://生产建筑
                        gameObject.AddComponent<ProductionBuilding>().Initialize(buildingName,new Vector2Int(v.x,v.y),span);
                        break;
                }
                
            }
            if (Input.GetMouseButtonDown(1))
            {
                tilemap.SetTile(lastPosition,lastTile);
                tilemap.color = Color.white;
                building = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                tilemap.color = translucent;
                building = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int v = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Vector2Int v_ = new Vector2Int(v.x,v.y);
                Dictionary<Vector2Int,BaseBuilding> register = BuildingManager.Instance.landUseRegister;
                if (register.ContainsKey(v_) && register[v_] != null)
                {
                    register[v_].PopUI();
                }
            }
        }
    }
    public void Select(string buildingName)
    {
        this.buildingName = buildingName;
        flip = false;
        tile = Resources.Load<Tile>("Tiles/"+buildingName);
    }
    public void Demolish(Vector2Int v)
    {
        tilemap.SetTile(new Vector3Int(v.x,v.y,1),null);
    }
    public bool CanBuild(string name, Vector3Int v, Vector2Int span)
    {
        var register = BuildingManager.Instance.landUseRegister;
        Vector2Int v_;
        for (int i = 0; i < span.x; i++)
        {
            for (int j = 0; j < span.y; j++)
            {
                v_ = new Vector2Int(v.x + i, v.y + j);
                if (register.ContainsKey(v_) && register[v_] != null)
                {
                    return false;
                }
                if ((BuildingManager.Instance.buildingInfoDict[name].restrictionMask & LandscapeManager.Instance.landscapeMap[v_]) == 0)
                {
                    return false;
                }
            }
        }
        //添加特殊规则

        return true;
    }
    //决定在建筑面板中是否置灰
    public bool CanSelect(string name)
    {
        if (BuildingManager.Instance.buildingCountDict[name] == BuildingManager.Instance.buildingInfoDict[name].buildingInfo.maxCount)
        {
            return false;
        }
        foreach(KeyValuePair<string,int> pair in BuildingManager.Instance.buildingInfoDict[name].upgradeRestrictionList[0])
        {
            if (BuildingManager.Instance.highestLevel[pair.Key] < pair.Value)
            {
                return false;
            }
        }
        return true;
    }
}
