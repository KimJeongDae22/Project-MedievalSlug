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
        float angle = ProjectileAngle(direction);

        GameObject proj = Instantiate(arrowPrefabs[0], spawnPosition.position, Quaternion.Euler(0, 0, angle));

        proj.GetComponent<ProjectileController>().Init(direction);
    }

    private float ProjectileAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

}
