using UnityEngine;

public class MeleeMonsterAttackState : MonsterBaseState
{
    public MeleeMonsterAttackState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StartAnimation(stateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        if (IsAnimationFinished())
        {
            if (IsTargetDetected() && IsTargetInAttackRange()) 
                stateMachine.ChangeState(stateMachine.attackState);
            else if (IsTargetDetected() && !IsTargetInAttackRange()) 
                stateMachine.ChangeState(stateMachine.chaseState);
            else if (!IsTargetDetected())
                stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    public override void ExitState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StopAnimation(stateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}