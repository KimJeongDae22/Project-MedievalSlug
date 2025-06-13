public abstract class MeleeMonsterBaseState : MonsterBaseState
{
    protected override MonsterStateMachine StateMachine => MeleeStateMachine;
    protected MeleeMonsterStateMachine MeleeStateMachine { get;}
    
    protected MeleeMonsterBaseState(MeleeMonsterStateMachine stateMachine) : base(stateMachine)
    {
        MeleeStateMachine = stateMachine;
    }

    public abstract override void EnterState();
    public abstract override void UpdateState();
    public abstract override void ExitState();
}