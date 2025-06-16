using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MonsterChaseState : MonsterBaseState
{
    public MonsterChaseState(MonsterStateMachine stateMachine) : base(stateMachine) {}

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
        //거리의 절대값을 구한다.
        float distance = Mathf.Abs(distanceX);
        //타겟이 사정거리 안에 있다면 공격 상태로 전환
        if (IsTargetInAttackRange())
        {
            StateMachine.ChangeState(StateMachine.AttackState);
        }
        //사정거리 밖이라면 사정거리까지 다가간다.
        else if (distance > StateMachine.Monster.MonsterData.AttackRange)
        {
            Vector2 currentPosition = StateMachine.transform.position;
            Vector2 targetPosition = new Vector2(StateMachine.target.position.x, currentPosition.y);

            StateMachine.transform.position = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                StateMachine.Monster.Speed * Time.deltaTime
            );
        }
     
    }

    public override void ExitState()
    {
        if (StateMachine.Monster.HasAnimator) 
            StopAnimation(StateMachine.Monster.AnimationHash.RunParameterHash);
    }
}