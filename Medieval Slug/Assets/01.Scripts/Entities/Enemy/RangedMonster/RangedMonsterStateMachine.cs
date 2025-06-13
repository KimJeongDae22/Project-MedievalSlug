using UnityEngine;

public class RangedMonsterStateMachine : MonsterStateMachine
{
    [field : Header("Ranged Monster")]
    [field : SerializeField] public RangedMonster RangedMonster { get; private set; }
    protected override void Reset()
    {
        base.Reset();
        RangedMonster = GetComponentInChildren<RangedMonster>();
    }

    protected void Start()
    {
        IdleState = new RangedMonsterIdleState(this);
        ChaseState = new RangedMonsterChaseState(this);
        AttackState = new RangedMonsterAttackState(this);
        ChangeState(IdleState);
    }
}