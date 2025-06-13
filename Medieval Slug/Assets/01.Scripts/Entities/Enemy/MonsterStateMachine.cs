using UnityEngine;

public abstract class MonsterStateMachine : MonoBehaviour
{
    [field : SerializeField] public Monster Monster { get; protected set; }
    [field : SerializeField] public Transform target { get; set; }
    [field : SerializeField] public LayerMask targetLayer { get; protected set; }

    private MonsterBaseState currentState;

    public MonsterBaseState IdleState { get; protected set; }
    public MonsterBaseState ChaseState { get; protected set; }
    public MonsterBaseState AttackState { get; protected set; }
    
    protected virtual void Reset()
    {
        Monster = GetComponentInChildren<Monster>();
        targetLayer = LayerMask.GetMask("Player");
        target = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(MonsterBaseState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
    /// <summary>
    /// 감지 확인용 범위 그리기
    /// </summary>
    void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Monster.MonsterData.DetectRange);
        
        Gizmos.color = Color.red;
        Vector2 origin = transform.position + Monster.MonsterData.RayOffset;
        float directionX = target.position.x - origin.x;
        Vector2 direction = directionX > 0 ? Vector2.right : Vector2.left;
        float attackRange = Monster.MonsterData.AttackRange;

        Gizmos.DrawLine(origin, origin + direction * attackRange);
    }
}
