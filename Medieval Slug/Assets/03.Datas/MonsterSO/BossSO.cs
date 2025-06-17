using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMonsterSO", menuName = "Entities/Boss", order = 0)]
public class BossSO : ScriptableObject
{
    [Header("Monster States")] 
    public int Health = 100;
    public int Damage = 1;
    public float PatternCooldown = 4f;
    public float SequentialFireDelay = 1.2f;
    public float AimingSpeed = 5f;
}