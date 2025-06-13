using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine) {}
    private float lastAttackTime = -Mathf.Infinity;

    public override void EnterState()
    {
        //공격하려는 시점이 저번 공격 시점 + 공격 쿨타임 보다 작다면 다시 추적 상태로(처음에는 무조건 공격)
        if (Time.time < lastAttackTime + StateMachine.Monster.MonsterData.AttackCooldown)
        {
            StateMachine.ChangeState(StateMachine.ChaseState);
            return;
        }
        //쿨타임이 지나 공격을 하면 그 시간을 이전 공격 시간으로 할당
        lastAttackTime = Time.time;
        
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        //공격애니메이션이 끝났을때 타겟이 감지가 안돼면 기본 상태로 아니면 추적 상태로
        //(쿨타임이 극단적으로 작아도 자체적으로 애니메이션으로 쿨타임)
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