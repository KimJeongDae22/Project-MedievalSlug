using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class VehicleItemCollector : MonoBehaviour
{
    [SerializeField] PlayerRangedHandler ranged;
    [SerializeField] PlayerStatHandler stats;
    public void Setup()
    {
        ranged = CharacterManager.Instance.GetComponent<PlayerRangedHandler>();
        stats = CharacterManager.Instance.GetComponent<PlayerStatHandler>();
    }

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


}
