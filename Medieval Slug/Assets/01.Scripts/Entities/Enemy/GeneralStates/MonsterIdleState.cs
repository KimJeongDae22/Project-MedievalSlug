using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.IdleParameterHash);
    }

    public override void UpdateState()
    {
        //타겟을 감지하면 추적 상태로
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