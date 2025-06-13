using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private List<GameObject> arrowPrefabs;

    public void Shoot(Vector2 direction, Transform spawnPosition, ProjectileType type)
    {
        float angle = ProjectileAngle(direction);
        GameObject proj = ObjectPoolManager.Instance.GetObject((int)type, spawnPosition.position, Quaternion.Euler(0, 0, angle));

        proj.GetComponent<ProjectileController>().Init(direction);
    }

    private float ProjectileAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}

