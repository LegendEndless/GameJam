using System.Collections.Generic;
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

    public TileBase mistTile;
    public TileBase unbuildableTile;



    public Grid grid;
    public Tilemap tilemap;
    public float padding = 40;

    //public GameObject buildingPrefab;  // 为添加建筑做测试 暂时先用不到

    Vector3Int lastPosition;
    TileBase lastTile;
    Color translucent;
    public bool isBuilding;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        translucent = new Color(1, 1, 1, 0.8f);
        lastTile = tilemap.GetTile(lastPosition);
        //Select("StarshipCenter");
        Build(BuildingManager.Instance.buildingInfoDict["StarshipCenter"].buildingInfo, new Vector3Int(-1, -1, 3), new Vector2Int(3, 3));
        tilemap.SetTile(new Vector3Int(-1, -1, 3), Resources.Load<Tile>("Tiles/Building/StarshipCenter"));
    }


    void Update()
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 300 * Time.unscaledDeltaTime * Input.GetAxis("Mouse ScrollWheel"), 2, 18);
        if (Input.mousePosition.x < padding)
        {
            Camera.main.transform.Translate(5 * Time.unscaledDeltaTime * Vector2.left);
        }
        if (Input.mousePosition.x > Screen.width - padding)
        {
            Camera.main.transform.Translate(5 * Time.unscaledDeltaTime * Vector2.right);
        }
        if (Input.mousePosition.y < padding)
        {
            Camera.main.transform.Translate(5 * Time.unscaledDeltaTime * Vector2.down);
        }
        if (Input.mousePosition.y > Screen.height - padding)
        {
            Camera.main.transform.Translate(5 * Time.unscaledDeltaTime * Vector2.up);
        }

        if (isBuilding)
        {
            Vector3Int v = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            v.z = 3;
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
                tilemap.SetTile(new Vector3Int(lastPosition.x, lastPosition.y, 4), null);

                tilemap.SetTile(lastPosition, lastTile);
                tilemap.RemoveTileFlags(lastPosition, TileFlags.LockColor);
                if (lastTile == mistTile)
                    tilemap.SetColor(lastPosition, Color.white);
                else
                    tilemap.SetColor(lastPosition, translucent);
                lastPosition = v;
                lastTile = tilemap.GetTile(lastPosition);
            }
            bool b = CanBuild(buildingName, v, span);

            tilemap.SetTile(v, tile);
            tilemap.RemoveTileFlags(v, TileFlags.LockColor);
            tilemap.SetColor(v, b ? Color.green : Color.red);

            if (lastTile != mistTile && lastTile != tile)
            {
                var v_ = new Vector3Int(v.x, v.y, 4);
                tilemap.SetTile(v_, lastTile);
                tilemap.RemoveTileFlags(v_, TileFlags.LockColor);
                tilemap.SetColor(v_, translucent);
            }

            if (Input.GetMouseButtonDown(0) && b)
            {
                lastTile = tile;
                Build(info, v, span);
                ExitBuildingMode();
            }
            if (Input.GetMouseButtonDown(1))
            {
                ExitBuildingMode();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (string resource in BuildingManager.Instance.list)
                {
                    ResourceManager.Instance.AddResource(resource, 10000);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int v = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Vector2Int v_ = new Vector2Int(v.x, v.y);
                Dictionary<Vector2Int, BaseBuilding> register = BuildingManager.Instance.landUseRegister;
                if (register.ContainsKey(v_) && register[v_] != null)
                {
                    register[v_].PopUI();
                }
            }
        }
    }
    public void ExitBuildingMode()
    {
        tilemap.SetTile(lastPosition, lastTile);
        tilemap.SetColor(lastPosition, Color.white);
        Vector3Int v_;
        for (int i = -LandscapeManager.Instance.maxSize; i <= LandscapeManager.Instance.maxSize; ++i)
        {
            for (int j = -LandscapeManager.Instance.maxSize; j <= LandscapeManager.Instance.maxSize; ++j)
            {
                v_ = new Vector3Int(i, j, 3);
                if (tilemap.GetTile(v_) != mistTile)
                {
                    tilemap.RemoveTileFlags(v_, TileFlags.LockColor);
                    tilemap.SetColor(v_, Color.white);
                }

            }
        }
        isBuilding = false;
    }
    public void Select(string buildingName)
    {
        this.buildingName = buildingName;
        flip = false;
        tile = Resources.Load<Tile>("Tiles/Building/" + buildingName);
        Vector3Int v_;
        for (int i = -LandscapeManager.Instance.maxSize; i <= LandscapeManager.Instance.maxSize; ++i)
        {
            for (int j = -LandscapeManager.Instance.maxSize; j <= LandscapeManager.Instance.maxSize; ++j)
            {
                v_ = new Vector3Int(i, j, 3);
                if (tilemap.GetTile(v_) != mistTile)
                {
                    tilemap.RemoveTileFlags(v_, TileFlags.LockColor);
                    tilemap.SetColor(v_, translucent);
                }
            }
        }
        isBuilding = true;
    }
    public void Demolish(Vector2Int v)
    {
        tilemap.SetTile(new Vector3Int(v.x, v.y, 3), null);
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
                if (!LandscapeManager.Instance.visibilityMap.ContainsKey(v_) || !LandscapeManager.Instance.buildabilityMap.ContainsKey(v_) || !LandscapeManager.Instance.visibilityMap[v_] || !LandscapeManager.Instance.buildabilityMap[v_])
                {
                    return false;
                }
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
        v_ = new Vector2Int(v.x, v.y);
        if (name == "WaterStation")
        {
            if (!(LandscapeManager.Instance.landscapeMap.ContainsKey(v_ + Vector2Int.left) && LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.left] == 16)
                && !(LandscapeManager.Instance.landscapeMap.ContainsKey(v_ + Vector2Int.right) && LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.right] == 16)
                && !(LandscapeManager.Instance.landscapeMap.ContainsKey(v_ + Vector2Int.up) && LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.up] == 16)
                && !(LandscapeManager.Instance.landscapeMap.ContainsKey(v_ + Vector2Int.down) && LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.down] == 16))
            {
                return false;
            }
        }
        else if (name == "EcoGarden" && IsNear(v_, span, new List<string> { "Mining", "OilWell", "NuclearPlant", "MantleSampling" }))
        {
            return false;
        }
        else if (name == "HighLab" && IsNear(v_, span, new List<string> { "WaterStation", "Corn" }))
        {
            return false;
        }
        else if ((new List<string> { "Mining", "OilWell", "NuclearPlant", "MantleSampling" }.Contains(name)) && IsNear(v_, span, new List<string> { "EcoGarden" }))
        {
            return false;
        }
        else if ((new List<string> { "WaterStation", "Corn" }.Contains(name)) && IsNear(v_, span, new List<string> { "HighLab" }))
        {
            return false;
        }
        else if (name == "MantleSampling")
        {
            if (LandscapeManager.Instance.landscapeMap[v_] != LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.right]
                || LandscapeManager.Instance.landscapeMap[v_] != LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.up]
                || LandscapeManager.Instance.landscapeMap[v_] != LandscapeManager.Instance.landscapeMap[v_ + Vector2Int.up + Vector2Int.right])
            {
                return false;
            }
            if (BuildingManager.Instance.sampling[LandscapeManager.Instance.landscapeMap[v_]])
            {
                return false;
            }
        }
        return true;
    }
    public bool IsNear(Vector2Int position, Vector2Int span, List<string> names)
    {
        Vector2Int u, v;
        int t;
        var register = BuildingManager.Instance.landUseRegister;
        foreach (var name in names)
        {
            for (int i = 0; i < span.x; i++)
            {
                for (int j = 0; j < span.y; j++)
                {
                    v = position + new Vector2Int(i, j);
                    for (int ii = -3; ii <= 3; ++ii)
                    {
                        t = 3 - Mathf.Abs(ii);
                        for (int jj = -t; jj <= t; ++jj)
                        {
                            u = v + new Vector2Int(ii, jj);
                            if (register.ContainsKey(u) && register[u].name == name)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    //决定在建筑面板中是否置灰
    public bool CanSelect(string name)
    {
        if (BuildingManager.Instance.buildingCountDict.ContainsKey(name) && BuildingManager.Instance.buildingCountDict[name] == BuildingManager.Instance.buildingInfoDict[name].buildingInfo.maxCount)
        {
            return false;
        }
        foreach (KeyValuePair<string, int> pair in BuildingManager.Instance.buildingInfoDict[name].upgradeRestrictionList[0])
        {
            if (!BuildingManager.Instance.highestLevelBuilding.ContainsKey(pair.Key) || BuildingManager.Instance.highestLevelBuilding[pair.Key].level < pair.Value)
            {
                return false;
            }
        }
        if (!BuildingManager.Instance.freeDict.ContainsKey(name) || !BuildingManager.Instance.freeDict[name])
        {
            foreach (var t in BuildingManager.Instance.buildingInfoDict[name].costList[0])
            {
                if (ResourceManager.Instance.GetResourceCount(t.Key) < t.Value)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void Build(BuildingInfo info, Vector3Int v, Vector2Int span)
    {
        GameObject gameObject = new GameObject(buildingName);

        switch (info.group)
        {
            case 1://标准生产建筑
                gameObject.AddComponent<ProductionBuilding>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                break;
            case 2://消耗物资的功能建筑
                switch (info.name)
                {
                    case "NursingHouse":
                        gameObject.AddComponent<NursingHouse>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "LAN":
                        gameObject.AddComponent<LAN>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "Gym":
                        gameObject.AddComponent<Gym>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "RobotFactory":
                        gameObject.AddComponent<RobotFactory>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                }
                break;
            case 3:
                switch (info.name)
                {
                    case "EcoGarden":
                        gameObject.AddComponent<EcoGarden>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "Magnetic":
                        gameObject.AddComponent<Magnetic>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "AirTower":
                        gameObject.AddComponent<AirTower>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "AILab":
                        gameObject.AddComponent<AILab>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "Apartment":
                        gameObject.AddComponent<Apartment>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "CellRepair":
                        gameObject.AddComponent<CellRepair>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                    case "RocketBase":
                        gameObject.AddComponent<RocketBase>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                        break;
                }
                break;
            case 4:
                gameObject.AddComponent<SingleProductionBuilding>().Initialize(buildingName, new Vector2Int(v.x, v.y), span);
                break;
        }
    }
}
