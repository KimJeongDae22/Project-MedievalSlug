public class MonsterHitState : MonsterBaseState
{
    public MonsterHitState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator)
        {
            SetAnimationTrigger(StateMachine.Monster.AnimationHash.HitParameterHash);
        }
    }
    public override void UpdateState()
    {
        if (IsAnimationFinished("Hit"))
        {
            StateMachine.ChangeState(!IsTargetDetected() ? StateMachine.IdleState : StateMachine.ChaseState);
        }
    }

    public override void ExitState()
    {
    }
}