using UnityEngine;

public class MeleeMonster : Monster
{
    [field: Header("Melee Monster")] 
    [field: SerializeField] public MonsterMeleeAttack MeleeCollider { get; private set; }

    protected override void Reset()
    {
        base.Reset();
        MeleeCollider = GetComponentInChildren<MonsterMeleeAttack>();
    }

    protected override void Awake()
    {
        base.Awake();
        if (MeleeCollider == null)
        { 
            MeleeCollider = GetComponentInChildren<MonsterMeleeAttack>();
        }
    }
}
