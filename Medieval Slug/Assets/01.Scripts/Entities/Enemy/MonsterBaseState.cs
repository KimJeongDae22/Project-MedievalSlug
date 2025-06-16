using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    protected virtual MonsterStateMachine StateMachine { get; }

    protected MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.StateMachine = stateMachine;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    
    /// <summary>
    /// 목표(플레이어)가 감지범위 안에 있는지 확인하는 메서드
    /// </summary>
    /// <returns></returns>
    protected bool IsTargetDetected()
    {
        Vector2 origin = StateMachine.transform.position;
        float detectionRadius = StateMachine.Monster.MonsterData.DetectRange;
        LayerMask targetLayer = StateMachine.targetLayer;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, detectionRadius, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                if (StateMachine.target == null)
                {
                    StateMachine.target = hit.transform;
                }
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 목표(플레이어)가 직선상에 있는지 확인하는 메서드
    /// </summary>
    /// <returns></returns>
    protected bool IsTargetInAttackRange()
    {
        Vector2 origin = StateMachine.Monster.transform.position + StateMachine.Monster.MonsterData.RayOffset;
        float directionX = StateMachine.target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, StateMachine.Monster.MonsterData.AttackRange, StateMachine.targetLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    protected void StartAnimation(int animatorHash)
    {
        StateMachine.Monster.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        StateMachine.Monster.Animator.SetBool(animatorHash, false);
    }
    
    protected void SetAnimationTrigger(int triggerHash)
    {
        StateMachine.Monster.Animator.SetTrigger(triggerHash);
    }
    
    /// <summary>
    /// tag가 붙은 애니메이션이 끝났는지 확인하는 불값 반환 함수
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    protected bool IsAnimationFinished(string tag)
    {
        AnimatorStateInfo stateInfo = StateMachine.Monster.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag(tag))
        { 
            return stateInfo.normalizedTime >= 1f;
        }
        return true;
    }
}
