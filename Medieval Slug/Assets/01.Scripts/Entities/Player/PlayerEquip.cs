using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 아이템 습득을 담당하는 클래스
/// </summary>
namespace Entities.Player
{
    [RequireComponent(typeof(PlayerStatHandler))]
    public class PlayerEquip : MonoBehaviour
    {
        [Header("Range Weapon Pivot (Child)")]
        [SerializeField] private Transform rangeWeaponPivot;
        [SerializeField] private GameObject basicBowPrefab;

        // 현재 장착된 무기
        private GameObject currentWeaponPrefab;
        private RangeWeaponHandler weaponHandler;
        private int currentAmmo;

        private Vector2 spawnOffset;
        private Transform spawnTransform;
        private PlayerStatHandler statHandler;

        private void Awake()
        {
            statHandler = GetComponent<PlayerStatHandler>();
            // 기본 활 인스턴스화
            GameObject bowInstance = Instantiate(basicBowPrefab, rangeWeaponPivot);
            bowInstance.transform.localPosition = Vector3.zero;
            bowInstance.transform.localRotation = Quaternion.identity;
           
            projectileHandler = bowInstance.GetComponent<ProjectileHandler>();
            if(projectileHandler == null)
            {
                Debug.LogError("ProjectileHandler missing on basicBowPrefab.");
            }

            // 발사 위치(SpawnPosition) 설정
            spawnTransform = bowInstance.transform.Find("SpawnPosition");
            if (spawnTransform != null)
            {
                spawnOffset = spawnTransform.localPosition;
            }
            else
            {
                Debug.LogError("SpawnPosition transform missing on basicBowPrefab.");
            }
        }

        /// <summary>
        /// 스코어 아이템 습득 시 
        /// OnScorePickup()은 임시로, 바로 Item쪽에서 호출해도 
        /// </summary>
        /// <param name="value"></param>
        public void OnScorePickup(int value)
        {
           // GameManager.Instance.AddScore(value);
        }
        /// <summary>
        /// 체력회복 아이템 습득 시
        /// Item.cs의 ApplyItemEffect()에서 이 함수를 호출하시면 될 것 같습니다.
        /// </summary>
        /// <param name="value"></param>
        public void OnHealthPickup(int value) => statHandler.ModifyStat(StatType.Health, value);

        /// <summary>
        /// 화살 아이템 습득 시 호출
        /// </summary>
        /// <param name="data">획득한 화살 타입 데이터</param>
        /// <param name="amount">획득 수량</param>
        public void OnArrowPickup(ProjectileData data)
        {
            // 같은 화살 타입 && 남은 탄약이 있을 때: 최대치 없이 누적
            if (currentProjectileData == data && currentAmmo > 0)
            {
                currentAmmo += data.MaxNum;
            }
            else
            {
                // 새로운 타입이거나 탄약이 0일 때 교체
                SetProjectileData(data);
            }

            // UI 갱신: OnAmmoChanged?.Invoke(currentAmmo);
        }

        /// <summary>
        /// 현재 사용 화살 타입 변경 및 탄약 초기화
        /// </summary>
        /// <param name="data">새 화살 데이터</param>
        /// <param name="initialAmmo">초기 탄약 수량</param>
        public void SetProjectileData(ProjectileData data)
        {
            currentProjectileData = data;
            //projectileHandler.SetProjectileData(data);
            currentAmmo = data.MaxNum;
        }

        /// <summary>
        /// 원거리 무리 발사
        /// </summary>
        // public void FireRange(Vector2 aimDirection)
        // {
        //     //if (projectileHandler == null || currentAmmo <= 0)
        //     //{
        //     //    Debug.Log("화실이 소진되었습니다.");
        //     //    return;
        //     //} 

        //     Vector2 dir = GetSnappedDirection(aimDirection);

        //     Vector3 worldOffset = rangeWeaponPivot.rotation * (Vector3)spawnOffset;
        //     spawnTransform.position = rangeWeaponPivot.position + worldOffset;
        //     projectileHandler.Shoot(dir);
        //     //currentAmmo--;

        //     // 탄약 소진 시 기본 무기로 복귀
        //     //if (currentAmmo <= 0)
        //     //    EquipRangeWeapon(basicBowPrefab);
        // }
        /// <summary>
        /// 방향키 입력에 따라 활 회전
        /// </summary>
        private Vector2 GetSnappedDirection(Vector2 rawInput)
        {
            if (rawInput.sqrMagnitude < 0.1f)
                rawInput = Vector2.right;

            // atan2 → degrees  → 0~360
            float angle = Mathf.Atan2(rawInput.y, rawInput.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            // 45° 단위 반올림
            float snapped = Mathf.Round(angle / 45f) * 45f;

            // 활 피벗 회전
            rangeWeaponPivot.rotation = Quaternion.Euler(0f, 0f, snapped);

            // 방향 벡터 반환
            float rad = snapped * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        /// <summary>
        /// 현재 탄약 수량 반환
        /// </summary>
        public int GetCurrentAmmo() => currentAmmo;
    }
}
