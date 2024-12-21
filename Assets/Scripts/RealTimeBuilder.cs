using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealTimeBuilder : MonoBehaviour
{
    public TileBase tile;
    public Grid grid;
    public Tilemap tilemap;
    Vector3Int lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int v = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        v.z = 2;
        if(lastPosition != v)
        {
            tilemap.SetTile(lastPosition, null);
            lastPosition = v;
        }
        tilemap.SetTile(v, tile);
        tilemap.SetColor(v, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            tilemap.SetTile(new Vector3Int(v.x,v.y,1),tile);
        }
        
    }
}
