using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> arrowPrefabs;

    public ProjectileData projectileData;

    public void Shoot(Vector2 direction) // 투사체 발사
    {
        float angle = ProjectileAngle(direction);

        GameObject proj = ObjectPoolManager.Instance.GetObject(ChoiceArrowType(), spawnPosition.position, Quaternion.Euler(0, 0, angle));

        proj.GetComponent<ProjectileController>().Init(direction);
    }

    private float ProjectileAngle(Vector2 direction) // 투사체 각도 설정
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public int ChoiceArrowType() // 어떤 투사체를 사용할건지 결정
    {
        int arrowTypeIndex = 0;

        return arrowTypeIndex;
    }

}
