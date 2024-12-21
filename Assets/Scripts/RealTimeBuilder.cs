using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealTimeBuilder : MonoBehaviour
{
    public TileBase tile;
    public Grid grid;
    public Tilemap tilemap;
    Vector3Int lastPosition;
    TileBase lastTile;
    Color translucent;
    bool building;
    // Start is called before the first frame update
    void Start()
    {
        translucent = new Color(1, 1, 1, 0.8f);
        lastTile = tilemap.GetTile(lastPosition);
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
