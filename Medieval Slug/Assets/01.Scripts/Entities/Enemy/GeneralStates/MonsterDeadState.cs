public class MonsterDeadState : MonsterBaseState
{
    public MonsterDeadState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator)
        {
            SetAnimationTrigger(StateMachine.Monster.AnimationHash.DeadParameterHash);
        }
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
}