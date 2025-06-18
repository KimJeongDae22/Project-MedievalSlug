using UnityEngine;

public class FlyingMonsterAttackState : MonsterBaseState
{
    private FlyingMonsterStateMachine FlyingStateMachine => StateMachine as FlyingMonsterStateMachine;
    public FlyingMonsterAttackState(FlyingMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
    private float lastAttackTime = -Mathf.Infinity;
    private Vector3 originalPosition = Vector3.zero;
    private Vector3 attackTargetPosition;
    private bool isReturning = false;

    public override void EnterState()
    {
        if (Time.time < lastAttackTime + StateMachine.Monster.MonsterData.AttackCooldown)
        {
            StateMachine.ChangeState(StateMachine.ChaseState);
            return;
        }
        
        lastAttackTime = Time.time;
        
        originalPosition = StateMachine.Monster.transform.position;
        attackTargetPosition = StateMachine.target.position;
        
        FlyingStateMachine.MeleeMonster.MeleeCollider.EnableCollider();
        
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        if (!isReturning && !IsReachedPlayer())
        {
            Vector3 direction = (attackTargetPosition - StateMachine.transform.position).normalized;

            StateMachine.transform.position += direction * (StateMachine.Monster.MonsterData.MoveSpeed * Time.deltaTime);
        }
        else if (!isReturning && IsReachedPlayer())
        {
            FlyingStateMachine.MeleeMonster.MeleeCollider.DisableCollider();
            isReturning = true;
            
            if (StateMachine.Monster.HasAnimator) 
                StopAnimation(StateMachine.Monster.AnimationHash.AttackParameterHash);
        }
        
        if (isReturning)
        {
            MoveToOriginalPosition();
            
            if (Vector3.Distance(StateMachine.Monster.transform.position, originalPosition) < 0.1f)
            {
                isReturning = false;
                StateMachine.ChangeState(StateMachine.ChaseState);
            }
        }
    }

    public override void ExitState()
    {
        
    }

    private bool IsReachedPlayer()
    {
        float distance = Vector2.Distance(StateMachine.transform.position, attackTargetPosition);
        
        return distance <= StateMachine.Monster.MonsterData.ReachDistance;
    }
    
    private void MoveToOriginalPosition()
    {
        Vector3 direction = (originalPosition - StateMachine.transform.position).normalized;

        StateMachine.transform.position += direction * (StateMachine.Monster.MonsterData.MoveSpeed * Time.deltaTime);
    }
}