
public class MonsterDeadState : MonsterBaseState
{
    public MonsterDeadState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StartAnimation(stateMachine.Monster.AnimationHash.DeadParameterHash);
    }

    public override void UpdateState() { }

    public override void ExitState() { }
}