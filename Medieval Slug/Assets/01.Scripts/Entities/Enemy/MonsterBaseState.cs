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

    protected bool IsTargetDetected()
    {
        Vector2 origin = stateMachine.transform.position;
        float detectionRadius = stateMachine.Monster.DetectRange;
        LayerMask targetLayer = stateMachine.targetLayer;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, detectionRadius, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
    
    protected bool IsTargetInAttackRange()
    {
        Vector2 origin = stateMachine.transform.position + stateMachine.Monster.DetectOffset;
        float directionX = stateMachine.target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, stateMachine.Monster.AttackRange, stateMachine.targetLayer);
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
    
    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = stateMachine.Monster.Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f;
    }
}
