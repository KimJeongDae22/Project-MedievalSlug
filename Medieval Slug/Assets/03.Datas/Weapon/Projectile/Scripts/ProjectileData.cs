using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Nomal,
    Fire,
    Ice,
    Poison,
    Cross,
    Null
}

public enum EffectType
{
    Nomal,
    Burn,
    Deceleration,
    Poisoning
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Projectile/Type,Stats")]
public class ProjectileData : ScriptableObject // 투사체 종류, 스텟
{
    [Header("Type")]
    public ProjectileType Type;

    [Header("Stats")]
    [Range(0, 20)]public float Damage;
    [Range(0, 20)]public float Range;
    [Range(0, 20)]public float AttackSpeed;
    [Range(0, 20)]public float ShotSpeed;
    [Range(0, 5)]public int ProjecileCount;
    [Range(0, 100)]public int MaxNum;

}
