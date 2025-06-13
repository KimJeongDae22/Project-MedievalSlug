using UnityEngine;

public class MeleeMonsterStateMachine : MonsterStateMachine
{
    [field : Header("Melee Monster")]
    [field : SerializeField] public MeleeMonster MeleeMonster { get; private set; }
    protected override void Reset()
    {
        base.Reset();
        MeleeMonster = GetComponentInChildren<MeleeMonster>();
    }

    protected void Start()
    {
        IdleState = new MeleeMonsterIdleState(this);
        ChaseState = new MeleeMonsterChaseState(this);
        AttackState = new MeleeMonsterAttackState(this);
        ChangeState(IdleState);
    }
}
