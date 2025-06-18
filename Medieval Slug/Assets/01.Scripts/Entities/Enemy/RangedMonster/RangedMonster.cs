using UnityEngine;

public class RangedMonster : Monster
{
    [field: Header("Ranged Monster")]
    [field: SerializeField] public MonsterRangedAttack RangedAttack { get; private set; }

    protected override void Reset()
    {
        base.Reset();
        RangedAttack = GetComponentInChildren<MonsterRangedAttack>();
    }

    protected override void Awake()
    {
        base.Awake();
        if (RangedAttack == null)
        {
            RangedAttack = GetComponentInChildren<MonsterRangedAttack>();
        }
    }
}
