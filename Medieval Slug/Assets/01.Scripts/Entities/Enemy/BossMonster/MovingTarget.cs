using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingTarget : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 1f;

    [SerializeField] private bool setSpeedRandom;

    void Update()
    {
        if (setSpeedRandom)
        {
            speed = Random.Range(0.1f, 1f);
        }
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(pointA, pointB, t);
    }

    public void DropBullet(Transform target, bool towardPlayer = false)
    {
        switch (towardPlayer)
        {
            case false:
                ProjectileManager.Instance.Shoot(Vector2.down, transform, ProjectileType.Slime);
                break;
            case true:
                Vector2 dir = (target.position - transform.position).normalized;
                ProjectileManager.Instance.Shoot(dir, transform, ProjectileType.Slime);
                break;
        }
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
