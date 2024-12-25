using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public TileBase[] tiles;
    public TileBase[] tiles_;
    public TileBase tile2;
    public TileBase tile3;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -20; i <= 20; ++i)
        {
            for (int j = -20; j <= 20; ++j)
            {
                tilemap.SetTile(new Vector3Int(i, j), tiles_[Random.Range(0,tiles_.Length)]);
                //tilemap.SetTile(new Vector3Int(i, j, 10), tiles[Random.Range(0, tiles.Length)]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
