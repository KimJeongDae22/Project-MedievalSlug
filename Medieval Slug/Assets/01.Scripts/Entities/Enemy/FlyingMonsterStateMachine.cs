public class FlyingMonsterStateMachine : MonsterStateMachine
{
    public MeleeMonster MeleeMonster => Monster as MeleeMonster;
    protected override void Start()
    {
        base.Start();
        ChaseState = new FlyingMonsterChaseState(this);
        AttackState = new FlyingMonsterAttackState(this);
    }
}