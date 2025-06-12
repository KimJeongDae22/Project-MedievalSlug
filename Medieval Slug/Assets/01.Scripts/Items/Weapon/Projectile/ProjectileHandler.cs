using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> arrowPrefabs;

    void Awake()
    {
        Shoot(transform.right);
    }

    public void Shoot(Vector2 direction)
    {
        GameObject proj = Instantiate(arrowPrefabs[0], spawnPosition.position, Quaternion.identity);
        proj.GetComponent<ProjectileController>().Init(direction);
    }

    // 화살 방향과 회전값 구하는 메서드

}
