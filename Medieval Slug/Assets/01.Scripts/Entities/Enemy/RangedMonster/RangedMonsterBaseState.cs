public abstract class RangedMonsterBaseState : MonsterBaseState
{
    protected override MonsterStateMachine StateMachine => RangedStateMachine;
    protected RangedMonsterStateMachine RangedStateMachine { get;}
    
    public RangedMonsterBaseState(RangedMonsterStateMachine stateMachine) : base(stateMachine)
    {
        RangedStateMachine = stateMachine;
    }

    public abstract override void EnterState();
    public abstract override void UpdateState();
    public abstract override void ExitState();
}