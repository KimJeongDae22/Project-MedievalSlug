using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어 아이템 획득 처리 클래스
/// </summary>
[RequireComponent(typeof(PlayerRangedHandler), typeof(PlayerStatHandler))]
public class PlayerItemCollector : MonoBehaviour
{
    [SerializeField] PlayerRangedHandler ranged;
    [SerializeField] PlayerStatHandler stats;

    void Reset()
    {
        if (ranged == null) ranged = GetComponent<PlayerRangedHandler>();
        if (stats == null) stats = GetComponent<PlayerStatHandler>();
    }
    #region 아이템 획득 로직

    /// <summary>
    /// 화살 아이템 획득 시 호출
    /// </summary>
    /// <param name="data"></param>
    public void OnArrowPickup(ProjectileData data)
    {
        ranged.HandleArrowPickup(data);
    }

    /// <summary>
    /// 체력 아이템 획득 시 호출
    /// </summary>
    /// <param name="value"></param>
    public void OnHealthPickup(int value)
    {
        stats.ModifyStat(StatType.Health, value);
    }
    #endregion

}
