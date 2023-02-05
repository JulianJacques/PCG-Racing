using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PCGGen : MonoBehaviour
{
    [SerializeField] private GameObject roadTile;
    [SerializeField] private GameObject groundTile;
    [SerializeField] private GameObject goalTile;
    [SerializeField] private PCGStyle style;
    private float tileSize = 20;

    private GameObject[,] tiles = new GameObject[500,500];
    private int gridsize = 500;

    private List<Vector3> path = new List<Vector3>();
    private List<Vector2Int> tileGridLocals = new List<Vector2Int>();

    [SerializeField]
    private int length = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2Int HallwayStart = Vector2Int.zero;
        //Vector2Int Direction = RandomDirection(style.directions.GetValue());
        Vector2Int Direction = Vector2Int.up;
        for (int i = 0; i < style.lengthOfMap.GetValue(); i++)
        {
            HallwayStart = SpawnHallway(HallwayStart, Direction,style.lengthOfHallway.GetValue());
            Vector2Int newDir = RandomDirection(style.directions.GetValue());
            
            while (newDir*-1 == Direction || newDir == Direction)
            {
                newDir = RandomDirection();
            }

            Direction = newDir;
        }

        if (length < style.minTiles)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        SpawnTile(HallwayStart);
        
        foreach (CarInputHandler carInputHandler in FindObjectsOfType<CarInputHandler>())
        {
            carInputHandler.setPath(path);
        }
        
        Vector3 goalPos = new Vector3(HallwayStart.x, HallwayStart.y, 0) * tileSize;
        goalPos.z = -1;
        Instantiate(goalTile, goalPos, Quaternion.identity);
        
        SpawnGround();
        
    }

    #region HallwaySpawnFunctions

    Vector2Int SpawnHallway(Vector2Int pos, int length)
    {
        Vector2Int dir = RandomDirection();
        for (int i = 0; i < length; i++)
        {
            if (GetTile(pos + (dir * i)) == null)
            {
                SpawnTile(pos + (dir * i));
            }
            else
            {
                return pos + (dir * (i - 1));
            }
        }

        return pos + dir * (length);
    }
    
    Vector2Int SpawnHallway(Vector2Int pos, Vector2Int dir, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (GetTile(pos + (dir * i)) == null)
            {
                SpawnTile(pos + (dir * i));
            }
            else
            {
                return pos + (dir * (i - 1));
            }
        }
        

        return pos + dir * (length);
    }

    
    Vector2Int SpawnHallway(int x, int y, int length)
    {
        Vector2Int pos = new Vector2Int(x, y);
        return SpawnHallway(pos, length);
    }
    
    Vector2Int SpawnHallway(int x, int y, Vector2Int dir, int length)
    {
        Vector2Int pos = new Vector2Int(x, y);
        return SpawnHallway(pos, dir, length);
    }
    #endregion

    #region tilesReferencesAndSpawning
    GameObject GetTile(int x, int y)
    {
        return tiles[gridsize / 2 + x, gridsize / 2 - y];
    }
    
    void SpawnTile(int x, int y)
    {
        GameObject tile = Instantiate(roadTile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
        length++;
    }
    
    void SpawnTileGround(int x, int y)
    {
        GameObject tile = Instantiate(groundTile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
        //length++;
    }
    
    void SpawnTileGround(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        GameObject tile = Instantiate(groundTile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
        //length++;
    }
    
    GameObject GetTile(Vector2Int pos)
    {
        return tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y];
    }
    
    GameObject SpawnTile(Vector2Int pos)
    {
        GameObject tile = Instantiate(roadTile, new Vector3(pos.x,pos.y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y] = tile;
        length++;
        path.Add(new Vector3(pos.x,pos.y,0)*tileSize);
        tileGridLocals.Add(pos);
        return tile;
    }


    #endregion

    #region HelperFunctionForRandom
    
    Vector2Int RandomDirection()
    {
        switch (DieRoll(3))
        {
            case 1: return Vector2Int.up;
            case 2: return Vector2Int.left;
            case 3: return Vector2Int.right;
            
            case 4: return Vector2Int.down;
            default: return Vector2Int.up;
        }
    }
    
    Vector2Int RandomDirection(int i)
    {
        switch (i)
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


    #endregion

    void SpawnGround()
    {
        List<Vector2Int> directionsToCheck = new List<Vector2Int>();
        directionsToCheck.Add(Vector2Int.down);
        directionsToCheck.Add(Vector2Int.up);
        
        directionsToCheck.Add(Vector2Int.left);
        directionsToCheck.Add(Vector2Int.right);
        
        directionsToCheck.Add(Vector2Int.left + Vector2Int.up);
        directionsToCheck.Add(Vector2Int.right + Vector2Int.up);
        
        directionsToCheck.Add(Vector2Int.left + Vector2Int.down);
        directionsToCheck.Add(Vector2Int.right + Vector2Int.down);
        
        foreach (Vector2Int pos in tileGridLocals)
        {
            foreach (Vector2Int vec in directionsToCheck)
            {
                if (GetTile(vec + pos) == null)
                {
                    SpawnTileGround(vec+pos);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
