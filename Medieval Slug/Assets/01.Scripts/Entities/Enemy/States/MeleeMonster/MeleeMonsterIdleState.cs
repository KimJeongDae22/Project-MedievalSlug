using UnityEngine;

public class MeleeMonsterIdleState : MonsterBaseState
{
    public MeleeMonsterIdleState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StartAnimation(stateMachine.Monster.AnimationHash.IdleParameterHash);
    }

    public override void UpdateState()
    {
        if (IsTargetDetected())
        {
            stateMachine.ChangeState(stateMachine.chaseState);
        }
    }

    public override void ExitState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StopAnimation(stateMachine.Monster.AnimationHash.IdleParameterHash);
    }
}