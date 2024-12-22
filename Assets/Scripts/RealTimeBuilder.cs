using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealTimeBuilder : MonoBehaviour
{
    public TileBase tile;
    public Grid grid;
    public Tilemap tilemap;
    public float padding = 100;

    //public GameObject buildingPrefab;  // 为添加建筑做测试 暂时先用不到

    Vector3Int lastPosition;
    TileBase lastTile;
    Color translucent;
    bool building;

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
            if (lastPosition != v)
            {
                tilemap.SetTile(lastPosition, lastTile);
                tilemap.SetColor(lastPosition,Color.white);
                lastPosition = v;
                lastTile = tilemap.GetTile(lastPosition);
            }
            bool empty = (lastTile == null);
            tilemap.SetTile(v, tile);
            tilemap.RemoveTileFlags(v, TileFlags.LockColor);
            tilemap.SetColor(v, empty ? Color.green : Color.red);

            if (Input.GetMouseButtonDown(0) && empty)
            {
                lastTile = tile;
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
}
