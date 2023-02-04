using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodRenderer : MonoBehaviour
{
    [Tooltip("Transform for the cockpit")]
    public Transform Cab;

    public List<Necelle> Nacelles = new List<Necelle>(); 


    private void OnDrawGizmos()
    {
        foreach (Necelle item in Nacelles)
        {
            item.Cab = Cab;
            item.UpdatePosition();
        }
    }
}


[System.Serializable]
public class Necelle
{
    public Transform Nacell;
    public Vector3 Offset;
    public LineRenderer Connector;
    [HideInInspector] public Transform Cab;

    public void UpdatePosition()
    {
        Nacell.localPosition = Cab.localPosition + Offset;
        
        Connector.SetPositions(new Vector3[] { Nacell.position, Cab.position });
    }
}
