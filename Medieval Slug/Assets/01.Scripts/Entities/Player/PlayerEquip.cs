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
        private ProjectileManager projectileManager;
        private int currentAmmo;
        private int maxAmmo;


        private PlayerStatHandler statHandler;

        private void Awake()
        {
            statHandler = GetComponent<PlayerStatHandler>();
            EquipRangeWeapon(basicBowPrefab);
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
        /// 아이템으로부터 무기를 습득할 때 호출
        /// Item.cs의 ApplyItemEffect()에서 이 함수를 호출하시면 될 것 같습니다.
        /// </summary>
        public void OnWeaponPickup(GameObject weaponPrefab)
        {
            // 같은 무기일 때 탄약 증가
            if (currentWeaponPrefab == weaponPrefab && currentAmmo > 0)
            {
                //int added = projectileHandler.StartingAmmo;  // prefab 데이터에서 가져올 예정
                //currentAmmo = currentAmmo //+ added;
            }
            else
            {
                EquipRangeWeapon(weaponPrefab);
            }
        }

        /// <summary>
        /// 무기 아이템 습득 시 호출
        /// </summary>
        /// <param name="handlerPrefab"></param>
        public void EquipRangeWeapon(GameObject RangeWeaponPrefab)
        {

            if (projectileManager != null)
                Destroy(projectileManager.gameObject);

            GameObject weaponInstance = Instantiate(RangeWeaponPrefab, rangeWeaponPivot);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;

            projectileManager = weaponInstance.GetComponent<ProjectileManager>();
            if (projectileManager == null)
            {
                Debug.LogError("ProjectileManager missing on weapon prefab.");
                return;
            }

            currentWeaponPrefab = RangeWeaponPrefab;
            //화살 수 초기화 로직
            //maxAmmo = projectileHandler.MaxAmmo;
            //currentAmmo = projectileHandler.StartingAmmo;

        }
        /// <summary>
        /// 아이템이 화살만 장착하는거면
        /// 화살만 교체하는 메소드
        /// </summary>
        /// <param name="projectile"></param>
        public void EquipProjectile(ProjectileData projectile)
        {
           //Todo : 투사체 데이터만 변경하는 로직
        }

        /// <summary>
        /// 원거리 무리 발사
        /// 방향은 플레이어 측에서 파라미터로 넘기겠습니다.
        /// </summary>
        public void FireRange(Vector2 aimDirection)
        {
            //if (projectileHandler == null || currentAmmo <= 0) return;

            Vector2 dir = aimDirection.sqrMagnitude > 0.01f
                ? aimDirection.normalized
                : (transform.localScale.x > 0 ? Vector2.right : Vector2.left);

            projectileManager.Shoot(dir);
            //currentAmmo--;

            // 탄약 소진 시 기본 무기로 복귀
            //if (currentAmmo <= 0)
            //    EquipRangeWeapon(basicBowPrefab);
        }
    }
}
