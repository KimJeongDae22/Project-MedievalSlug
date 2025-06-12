public class MeleeMonsterStateMachine : MonsterStateMachine
{
    protected void Start()
    {
        idleState = new MeleeMonsterIdleState(this);
        chaseState = new MeleeMonsterChaseState(this);
        attackState = new MeleeMonsterAttackState(this);

        ChangeState(idleState);
    }
}
