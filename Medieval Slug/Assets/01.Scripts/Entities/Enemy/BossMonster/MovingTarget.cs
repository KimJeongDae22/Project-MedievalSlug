using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 1f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(pointA, pointB, t);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(pointA, Vector3.one * 0.2f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(pointB, Vector3.one * 0.2f);
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(pointA, pointB);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
