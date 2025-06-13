using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        if (IsAnimationFinished("Attack"))
        {
            if (IsTargetDetected() && IsTargetInAttackRange()) 
                StateMachine.ChangeState(StateMachine.AttackState);
            else if (IsTargetDetected() && !IsTargetInAttackRange()) 
                StateMachine.ChangeState(StateMachine.ChaseState);
            else if (!IsTargetDetected())
                StateMachine.ChangeState(StateMachine.IdleState);
        }
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}