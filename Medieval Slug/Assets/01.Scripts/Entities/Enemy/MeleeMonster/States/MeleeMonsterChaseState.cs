using UnityEngine;

public class MeleeMonsterChaseState : MeleeMonsterBaseState
{
    public MeleeMonsterChaseState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    private float attackCooldown;

    public override void EnterState()
    {
        attackCooldown = StateMachine.Monster.MonsterData.AttackCooldown;
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.RunParameterHash);
    }

    public override void UpdateState()
    {
        if (!IsTargetDetected())
        {
            StateMachine.ChangeState(StateMachine.IdleState);
        }
        
        float distanceX = (StateMachine.transform.position.x - StateMachine.target.position.x);
        
        if (distanceX < 0)
        {
            StateMachine.Monster.Sprite.flipX = true;
            MeleeStateMachine.MeleeMonster.FlipMeleeCollider(true);
        }
        else
        {
            StateMachine.Monster.Sprite.flipX = false;
            MeleeStateMachine.MeleeMonster.FlipMeleeCollider(false);
        }
        
        float distance = Mathf.Abs(distanceX);
        
        if (IsTargetInAttackRange())
        {
            if (attackCooldown < StateMachine.Monster.MonsterData.AttackCooldown)
            {
                attackCooldown += Time.deltaTime;
            }
            else
            {
                attackCooldown = 0f;
                StateMachine.ChangeState(StateMachine.AttackState);
            }
        }
        else if (distance > StateMachine.Monster.MonsterData.AttackRange)
        {
            StateMachine.transform.position = Vector2.MoveTowards(
                (StateMachine.transform.position),
                StateMachine.target.position,
                StateMachine.Monster.MonsterData.MoveSpeed * Time.deltaTime
            );
        }
     
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.RunParameterHash);
    }
}