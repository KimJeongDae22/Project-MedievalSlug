using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBaseState
{
    protected MonsterStateMachine stateMachine;
    private int animHash;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
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
        Vector2 origin = stateMachine.transform.position;
        float detectionRadius = stateMachine.Monster.MonsterData.DetectRange;
        LayerMask targetLayer = stateMachine.targetLayer;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, detectionRadius, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                if (stateMachine.target == null)
                {
                    stateMachine.target = hit.transform;
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
        Vector2 origin = stateMachine.transform.position + stateMachine.Monster.MonsterData.RayOffset;
        float directionX = stateMachine.target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, stateMachine.Monster.MonsterData.AttackRange, stateMachine.targetLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Monster.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Monster.Animator.SetBool(animatorHash, false);
    }
    
    protected bool IsAnimationFinished(string tag)
    {
        AnimatorStateInfo stateInfo = stateMachine.Monster.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag(tag))
        { 
            return stateInfo.normalizedTime >= 1f;
        }
        return true;
    }
}
