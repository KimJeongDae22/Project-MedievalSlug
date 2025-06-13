using UnityEngine;

public class MeleeMonsterIdleState : MeleeMonsterBaseState
{
    public MeleeMonsterIdleState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.IdleParameterHash);
    }

    public override void UpdateState()
    {
        if (IsTargetDetected())
        {
            StateMachine.ChangeState(StateMachine.ChaseState);
        }
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.IdleParameterHash);
    }
}