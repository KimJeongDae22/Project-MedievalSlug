public class RangedMonsterAttackState : RangedMonsterBaseState
{
    public RangedMonsterAttackState(RangedMonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}