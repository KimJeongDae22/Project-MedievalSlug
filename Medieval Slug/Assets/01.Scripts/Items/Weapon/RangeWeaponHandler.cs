using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 하나의 "활" 프리팹에 부착되어 실제 발사 로직을 담당.
/// ProjectileType만 바꾸어 다양한 화살을 발사한다.
/// </summary>
public class RangeWeaponHandler : MonoBehaviour
{
    [Header("Spawn & Data")]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ProjectileType projectileType = ProjectileType.Nomal;


    //초기 화살
    [SerializeField]public ProjectileData projectileData;
    /// <summary>
    /// PlayerRnagedHandler → Fire() 호출
    /// </summary>
    public void Fire(Vector2 aimDirection)
    {
        if (aimDirection.sqrMagnitude < 0.01f)
            aimDirection = transform.right; // 입력이 없으면 캐릭터 정면

        Vector2 dir = GetSnappedDirection(aimDirection);

        // 활 자체 회전 (Z-축)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        ProjectileManager.Instance.Shoot(dir, spawnPosition, projectileType);
    }

    /// <summary>
    /// PlayerRnagedHandler 현재 장착 화살 타입을 변경할 때 호출
    /// </summary>
    public void SetProjectileType(ProjectileType newType) => projectileType = newType;

    /* ---------- 내부 유틸 ---------- */
    private Vector2 GetSnappedDirection(Vector2 rawInput)
    {
        // 45° 간격 스냅
        float angle = Mathf.Atan2(rawInput.y, rawInput.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        float snapped = Mathf.Round(angle / 45f) * 45f;
        float rad = snapped * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}