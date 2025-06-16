using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMonsterSO", menuName = "Entities/Boss", order = 0)]
public class BossSO : ScriptableObject
{
    [Header("Monster States")] 
    public int Health = 100;
    public int Damage = 1;
    public float AttackCooldown = 0.5f;
    public float FireDelay = 1f;
}