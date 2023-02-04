using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PCGGen : MonoBehaviour
{
    [SerializeField] private GameObject roadtile;
    [SerializeField] private GameObject groundtile;
    private float tileSize = 20;

    private GameObject[,] tiles = new GameObject[500,500];
    private int gridsize = 500;

    private List<Vector3> path = new List<Vector3>();

    [SerializeField]
    private int length = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2Int HallwayStart = Vector2Int.zero;
        Vector2Int Direction = Vector2Int.up;
        for (int i = 0; i < 10; i++)
        {
            HallwayStart = SpawnHallway(HallwayStart, Direction,DieRoll(10)+4);
            Vector2Int newDir = RandomDirection();
            
            while (newDir*-1 == Direction || newDir == Direction)
            {
                newDir = RandomDirection();
            }

            Direction = newDir;
        }

        if (length < 40)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        Debug.Log(length);
        SpawnGround();

        foreach (CarInputHandler carInputHandler in FindObjectsOfType<CarInputHandler>())
        {
            carInputHandler.setPath(path);
        }
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
        GameObject tile = Instantiate(roadtile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
        length++;
    }
    
    void SpawnTileGround(int x, int y)
    {
        GameObject tile = Instantiate(groundtile, new Vector3(x,y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + x, gridsize / 2 - y] = tile;
        //length++;
    }
    
    GameObject GetTile(Vector2Int pos)
    {
        return tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y];
    }
    
    void SpawnTile(Vector2Int pos)
    {
        GameObject tile = Instantiate(roadtile, new Vector3(pos.x,pos.y,0)*tileSize, Quaternion.identity);
        tiles[gridsize / 2 + pos.x, gridsize / 2 - pos.y] = tile;
        length++;
        path.Add(new Vector3(pos.x,pos.y,0)*tileSize);
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

    int DieRoll(int sides)
    {
        return Random.Range(1, sides + 1);
    }


    #endregion

    void SpawnGround()
    {
        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 50; j++)
            {
                if (GetTile(i, j) == null)
                {
                    SpawnTileGround(i,j);
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
