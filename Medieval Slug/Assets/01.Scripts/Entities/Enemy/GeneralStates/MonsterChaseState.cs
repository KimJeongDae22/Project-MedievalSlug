using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MonsterChaseState : MonsterBaseState
{
    public MonsterChaseState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    private Vector3 rotate = new Vector3(0, 180, 0);
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
            StateMachine.Monster.gameObject.transform.rotation = Quaternion.Euler(rotate);
        }
        else
        {
            StateMachine.Monster.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
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