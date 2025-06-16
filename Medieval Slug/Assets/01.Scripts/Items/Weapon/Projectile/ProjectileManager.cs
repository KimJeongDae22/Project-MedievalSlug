using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private GameObject[] arrowPrefabs;

    /// <summary>
    /// dir : 월드 방향, spawn : 발사 위치/회전 기준, type : 화살 종류
    /// </summary>
    public void Shoot(Vector2 dir, Transform spawn, ProjectileType type = ProjectileType.Nomal)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        int index = Mathf.Clamp((int)type, 0, arrowPrefabs.Length - 1);
        GameObject proj = ObjectPoolManager.Instance.GetObject(index, spawn.position, Quaternion.Euler(0, 0, angle));
        proj.GetComponent<ProjectileController>().Init(dir);
    }
}


