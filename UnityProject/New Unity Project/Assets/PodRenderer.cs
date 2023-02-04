using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodRenderer : MonoBehaviour
{
    [Tooltip("Transform for the cockpit")]
    public Transform Cab;

    public List<Necelle> Nacelles = new List<Necelle>();

    public void Awake()
    {
        foreach (Necelle item in Nacelles)
            item.InitiateNacelle(Cab);

    }
    private void FixedUpdate()
    {
        foreach (Necelle item in Nacelles)
        {
            item.UpdatePosition();
        }

    }
    private void LateUpdate()
    {
        foreach (Necelle item in Nacelles)
        {
            item.DrawNacelle();
        }

    }
    private void OnDrawGizmos()
    {
        foreach (Necelle item in Nacelles)
        {
            item.InitiateNacelle(Cab);

            item.UpdatePosition();
        }
    }
}


[System.Serializable]
public class Necelle
{
    Rigidbody2D rb;
    public Transform Nacell;
    public Vector3 Offset;
    public LineRenderer Connector;
    Transform Cab;

    //[Range(0, 1)]
    public float Interpolate;
    Vector3 targetpos;

    public void InitiateNacelle(Transform cab)
    {
        Cab = cab;
        rb = Nacell.GetComponent<Rigidbody2D>();
    }

    public void DrawNacelle()
    {
        //Nacell.position = targetpos;

        Connector.SetPositions(new Vector3[] { Nacell.position, Cab.position });
    }
    public void UpdatePosition()
    {
        targetpos = Vector3.Lerp(Nacell.position, Cab.position + (Offset.x * Cab.right) + (Offset.y * Cab.up), Interpolate);
        rb.MovePosition(targetpos);
        Connector.SetPositions(new Vector3[] { Nacell.position, Cab.position });
    }
}
