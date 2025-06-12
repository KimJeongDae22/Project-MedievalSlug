public class MeleeMonsterStateMachine : MonsterStateMachine
{
    protected void Start()
    {
        IdleState = new MeleeMonsterIdleState(this);
        ChaseState = new MeleeMonsterChaseState(this);
        AttackState = new MeleeMonsterAttackState(this);
        DeadState = new MonsterDeadState(this);
        ChangeState(IdleState);
    }
}
