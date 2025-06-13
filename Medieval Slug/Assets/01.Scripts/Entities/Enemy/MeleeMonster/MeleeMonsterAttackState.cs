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
        if (IsAnimationFinished("Attack"))
        {
            if (IsTargetDetected() && IsTargetInAttackRange()) 
                stateMachine.ChangeState(stateMachine.AttackState);
            else if (IsTargetDetected() && !IsTargetInAttackRange()) 
                stateMachine.ChangeState(stateMachine.ChaseState);
            else if (!IsTargetDetected())
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void ExitState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StopAnimation(stateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}