using UnityEngine;

public class FlyingMonsterChaseState : MonsterBaseState
{
    private FlyingMonsterStateMachine FlyingStateMachine => StateMachine as FlyingMonsterStateMachine;
    public FlyingMonsterChaseState(FlyingMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private Vector3 rotate = new Vector3(0, 180, 0);
    
    public override void EnterState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StartAnimation(StateMachine.Monster.AnimationHash.RunParameterHash);
    }

    public override void UpdateState()
    {
        //타겟을 감지 못했으면 기본상태로
        if (!IsTargetDetected())
        {
            StateMachine.ChangeState(StateMachine.IdleState);
        }
        //x좌표 거리를 재고
        float distanceX = (StateMachine.transform.position.x - StateMachine.target.position.x);
        //오른쪽에 있으면 반대편으로 돌아본다
        if (distanceX < 0)
        {
            StateMachine.Monster.gameObject.transform.rotation = Quaternion.Euler(rotate);
        }
        else
        {
            StateMachine.Monster.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        if (IsTargetInRange())
        {
            StateMachine.ChangeState(StateMachine.AttackState);
        }
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.RunParameterHash);
    }

    private bool IsTargetInRange()
    {
        Vector2 origin = StateMachine.Monster.transform.position + StateMachine.Monster.MonsterData.RayOffset;
        Vector2 direction = (Vector2)StateMachine.target.position - origin;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, StateMachine.Monster.MonsterData.AttackRange, StateMachine.targetLayer);
        Debug.DrawRay(origin, direction, Color.red);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}