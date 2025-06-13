using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine) {}
    private float lastAttackTime = -Mathf.Infinity;

    public override void EnterState()
    {
        if (Time.time < lastAttackTime + StateMachine.Monster.MonsterData.AttackCooldown)
        {
            StateMachine.ChangeState(StateMachine.ChaseState);
            return;
        }

        lastAttackTime = Time.time;
        
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        if (IsAnimationFinished("Attack"))
        {
            if (!IsTargetDetected())
                StateMachine.ChangeState(StateMachine.IdleState);
            else
                StateMachine.ChangeState(StateMachine.ChaseState);
        }
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}