using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(menuName = "My Assets/PCGStyle")]
public class PCGStyle : ScriptableObject
{
    public Baggie lengthOfMap;
    public Baggie lengthOfHallway;
    public Baggie directions;
    public int minTiles;
}

[Serializable]
public class Baggie
{
    
    public List<Vector2Int> marbles = new List<Vector2Int>();

    public int GetValue()
    {
        CheckIfDoable();
        int val = Random.Range(0, marbles.Count);
        while (marbles[val].y != 0)
        {
            val = Random.Range(0, marbles.Count);
        }

        marbles[val] = new Vector2Int(marbles[val].x, 1);
        return marbles[val].x;
    }

    private void CheckIfDoable()
    {
        foreach (Vector2Int vec in marbles)   
        {
            if (vec.y == 0)
                return;
        }

        for (int i = 0; i < marbles.Count; i++)
        {
            marbles[i] = new Vector2Int(marbles[i].x, 0);
        }
    }
}
