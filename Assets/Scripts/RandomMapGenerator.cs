using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public TileBase tile1;
    public TileBase tile2;
    public TileBase tile3;
    // Start is called before the first frame update
    void Start()
    {
        tilemap.ClearAllTiles();
        for (int i = -10; i <= 10; ++i)
        {
            for (int j = -10; j <= 10; ++j)
            {
                tilemap.SetTile(new Vector3Int(i, j), Random.Range(0, 2) == 1 ? tile1 : tile2);
                if(Random.Range(0, 5) == 1)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 1),tile3);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
