using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ProjectileType equippedType = ProjectileType.Nomal; // 임시
    

        public void FireRange(Vector2 aimDirection) // 화살 발사
        {
            //if (projectileHandler == null || currentAmmo <= 0) return;

            Vector2 dir = aimDirection.sqrMagnitude > 0.01f
                ? aimDirection.normalized
                : (transform.localScale.x > 0 ? Vector2.right : Vector2.left);

            ProjectileManager.Instance.Shoot(dir, spawnPosition, equippedType);
            //currentAmmo--;

            // 탄약 소진 시 기본 무기로 복귀
            //if (currentAmmo <= 0)
            //    EquipRangeWeapon(basicBowPrefab);
        }

    public void Equip(ProjectileType newType) // 화살 변경
    {
        equippedType = newType;
    }
}
