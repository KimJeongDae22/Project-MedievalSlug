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
    public class PlayerRangedHandler : MonoBehaviour
    {
        [Header("Bow Prefab (고정)")]
        [SerializeField] private GameObject bowPrefab;
        [SerializeField] private Transform RangeWeaponPivot;

        [Header("남은 화살 수")]
        [SerializeField]private int currentAmmo;

        private RangeWeaponHandler bowHandler;
        private ProjectileData currentArrowData;
        private float nextFireTime = 0f;
        private bool isBursting;

        private void Awake()
        {
            SpawnBow();
        }

        /// <summary>
        /// RangeWeapon의 Fire 외부 API
        /// </summary>
        /// <param name="aimDirection"></param>
        public void Fire(Vector2 aimDir)
        {
            if (currentAmmo <= 0) return;
            if (currentArrowData.AttackSpeed <= 0f) return;

            //발사 시퀀스 쿨타임 검사 
            float interval = (currentArrowData.AttackSpeed > 0f)
                           ? 1f / currentArrowData.AttackSpeed
                           : 0;
            if (Time.time < nextFireTime) return;

            nextFireTime = Time.time + interval;
            if (!isBursting) StartCoroutine(FireBurst(aimDir.normalized));
        }

        IEnumerator FireBurst(Vector2 dir)
        {
            isBursting = true;
            int burst = Mathf.Max(1, currentArrowData.ProjecileCount);
            for (int i = 0; i < burst && currentAmmo > 0; i++)
            {
                bowHandler.Fire(dir);        // 실제 투사체 생성
                currentAmmo--;

                /* 연사 간 미세 지연 */
                if (i < burst - 1)
                    yield return new WaitForSeconds(0.11f);
            }
            isBursting = false;
        }

        /// <summary>
        /// 시작 시 기본활 생성
        /// </summary>
        private void SpawnBow()
        {
            if (bowHandler != null) return; // 이미 있음
            GameObject bow = Instantiate(bowPrefab, RangeWeaponPivot);
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

        public void HandleArrowPickup(ProjectileData data)
        {
            if (currentArrowData == data && currentAmmo > 0)
                currentAmmo += data.MaxNum;
            else
                SetArrowData(data);
        }

        /// <summary>
        /// 남은 화살 수 반환
        /// 임시용
        /// </summary>
        /// <returns></returns>
        public int GetCurrentAmmo() => currentAmmo;
    }
}

