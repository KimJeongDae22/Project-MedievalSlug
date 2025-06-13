using UnityEngine;

public class FlyingMonsterAttackState : MonsterBaseState
{
    private FlyingMonsterStateMachine FlyingStateMachine => StateMachine as FlyingMonsterStateMachine;
    public FlyingMonsterAttackState(FlyingMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
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
        //원래 위치를 저장하고 플레이어에게 다가가 공격
        if (IsReachedPlayer())
        {
            FlyingStateMachine.MeleeMonster.DisableCollider();
            //다시 원래 위치로 복귀하고 플레이어 추적
        }
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    private bool IsReachedPlayer()
    {
        //거리비교 및 반환
        return false;
    }
}