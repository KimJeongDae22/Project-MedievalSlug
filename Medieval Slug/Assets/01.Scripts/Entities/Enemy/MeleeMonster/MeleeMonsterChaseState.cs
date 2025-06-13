using UnityEngine;

public class MeleeMonsterChaseState : MonsterBaseState
{
    public MeleeMonsterChaseState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    private float attackCooldown;

    public override void EnterState()
    {
        attackCooldown = stateMachine.Monster.MonsterData.AttackCooldown;
        if (stateMachine.Monster.HasAnimator) 
            StartAnimation(stateMachine.Monster.AnimationHash.RunParameterHash);
    }

    public override void UpdateState()
    {
        if (!IsTargetDetected())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        
        float distanceX = (stateMachine.transform.position.x - stateMachine.target.position.x);
        
        if (distanceX < 0)
        {
            stateMachine.Monster.Sprite.flipX = true;
            stateMachine.Monster.meleeCollider.FlipMeleeCollider(true);
        }
        else
        {
            stateMachine.Monster.Sprite.flipX = false;
            stateMachine.Monster.meleeCollider.FlipMeleeCollider(false);
        }
        
        float distance = Mathf.Abs(distanceX);
        
        if (IsTargetInAttackRange())
        {
            if (attackCooldown < stateMachine.Monster.MonsterData.AttackCooldown)
            {
                attackCooldown += Time.deltaTime;
            }
            else
            {
                attackCooldown = 0f;
                stateMachine.ChangeState(stateMachine.AttackState);
            }
        }
        else if (distance > stateMachine.Monster.MonsterData.AttackRange)
        {
            stateMachine.transform.position = Vector2.MoveTowards(
                (stateMachine.transform.position),
                stateMachine.target.position,
                stateMachine.Monster.MonsterData.MoveSpeed * Time.deltaTime
            );
        }
     
    }

    public override void ExitState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StopAnimation(stateMachine.Monster.AnimationHash.RunParameterHash);
    }
}