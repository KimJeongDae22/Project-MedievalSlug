using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 아이템 습득을 담당하는 클래스
/// </summary>
namespace Entities.Player
{
    /// <summary>
    /// 무기는 고정(활 1개). 화살(ProjectileData)만 교체·탄약 관리.
    /// </summary>
    [RequireComponent(typeof(PlayerStatHandler))]
    public class PlayerEquip : MonoBehaviour
    {
        [Header("Bow Prefab (고정)")]
        [SerializeField] private GameObject bowPrefab;
        [SerializeField] private Transform RangeWaeponPivot;

        private RangeWeaponHandler bowHandler;
        private ProjectileData currentArrowData;
        [SerializeField]private int currentAmmo;
        private PlayerStatHandler statHandler;

        private void Awake()
        {
            statHandler = GetComponent<PlayerStatHandler>();
            SpawnBow();
        }

        /// <summary>
        /// RangeWeapon의 Fire 외부 API
        /// </summary>
        /// <param name="aimDirection"></param>
        public void Fire(Vector2 aimDirection)
        {
            if (currentAmmo <= 0) return;
            bowHandler.Fire(aimDirection);
            currentAmmo--;
            // TODO: UI 이벤트 OnAmmoChanged?.Invoke(currentAmmo);
        }

        #region 아이템 획득 로직
        /// <summary>
        /// 화살 아이템 획득 시 호출
        /// </summary>
        public void OnArrowPickup(ProjectileData data)
        {
            // 동일 화살이면 탄약 누적, 아니면 교체
            if (currentArrowData == data && currentAmmo > 0)
                currentAmmo += data.MaxNum;
            else
                SetArrowData(data);
        }

        public void OnHealthPickup(int value) => statHandler.ModifyStat(StatType.Health, value);

        #endregion

        /// <summary>
        /// 시작 시 기본활 생성
        /// </summary>
        private void SpawnBow()
        {
            if (bowHandler != null) return; // 이미 있음
            GameObject bow = Instantiate(bowPrefab, RangeWaeponPivot);
            bow.transform.localPosition = Vector3.zero;
            bow.transform.localRotation = Quaternion.identity;
            bowHandler = bow.GetComponent<RangeWeaponHandler>();
            SetArrowData(bowHandler.projectileData);
            if (bowHandler == null)
                Debug.LogError("Bow Prefab에 RangeWeaponHandler가 없습니다.");
        }

        /// <summary>
        /// 화살 데이터 교체 
        /// </summary>
        /// <param name="data"></param>
        private void SetArrowData(ProjectileData data)
        {
            currentArrowData = data;
            currentAmmo = data.MaxNum;
            bowHandler.SetProjectileType(data.Type);
            // TODO: UI 이벤트 OnAmmoChanged?.Invoke(currentAmmo);
        }

        /// <summary>
        /// 남은 화살 수 반환
        /// 임시용
        /// </summary>
        /// <returns></returns>
        public int GetCurrentAmmo() => currentAmmo;
    }
}

