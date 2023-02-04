using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGGen : MonoBehaviour
{
    [SerializeField] private GameObject roadtile;
    private float tileSize = 10;

    private GameObject[,] tiles = new GameObject[500,500];
    private int gridsize = 500;

    // Start is called before the first frame update
    void Start()
    {
        Vector2Int HallwayStart = Vector2Int.zero;
        for (int i = 0; i < 10; i++)
        {
            HallwayStart = SpawnHallway(HallwayStart, DieRoll(20));
        }
    }

    #region HallwaySpawnFunctions

    Vector2Int SpawnHallway(Vector2Int pos, int length)
    {
        Vector2Int dir = RandomDirection();
        for (int i = 0; i < length; i++)
        {
            SpawnTile(pos + (dir * i));
        }

        return pos + dir * (length);
    }

    
    Vector2Int SpawnHallway(int x, int y, int length)
    {
        Vector2Int pos = new Vector2Int(x, y);
        return SpawnHallway(pos, length);
    }
    #endregion

    #region tilesReferencesAndSpawning
    GameObject GetTile(int x, int y)
    {
        return tiles[gridsize / 2 + x, gridsize / 2 - y];
    }
    
    void SpawnTile(int x, int y)
    {
        GameObject tile = Instantiate(roadtile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
    }
    
    GameObject GetTile(Vector2Int pos)
    {
        return tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y];
    }
    
    void SpawnTile(Vector2Int pos)
    {
        GameObject tile = Instantiate(roadtile, new Vector3(pos.x,pos.y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y] = tile;
    }


    #endregion

    Vector2Int RandomDirection()
    {
        switch (DieRoll(4))
        {
            case 1: return Vector2Int.up;
            case 2: return Vector2Int.left;
            case 3: return Vector2Int.right;
            case 4: return Vector2Int.down;
            default: return Vector2Int.up;
        }
    }

    int DieRoll(int sides)
    {
        return Random.Range(1, sides + 1);
    }

}
