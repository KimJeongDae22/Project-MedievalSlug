public abstract class MeleeMonsterBaseState : MonsterBaseState
{
    protected override MonsterStateMachine StateMachine => MeleeStateMachine;
    protected MeleeMonsterStateMachine MeleeStateMachine { get;}
    
    protected MeleeMonsterBaseState(MeleeMonsterStateMachine stateMachine) : base(stateMachine)
    {
        MeleeStateMachine = stateMachine;
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
}