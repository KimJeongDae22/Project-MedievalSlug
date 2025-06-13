using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMonsterSO", menuName = "Entities/Monster", order = 0)]
public class MonsterSO : ScriptableObject
{
    [Header("Monster States")] 
    public int Health = 1;
    public int Damage = 1;
    public float AttackCooldown = 0.5f;
    public float MoveSpeed = 2f;
    public float DetectRange = 10f;
    public Vector3 RayOffset = Vector3.zero;
    public float AttackRange = 1f;
}