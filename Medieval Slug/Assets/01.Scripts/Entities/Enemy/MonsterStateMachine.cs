using UnityEngine;

public abstract class MonsterStateMachine : MonoBehaviour
{
    [field : SerializeField] public Monster Monster { get; private set; }
    [field : SerializeField] public Transform target { get; set; }
    [field : SerializeField] public LayerMask targetLayer { get; private set; }
    
    protected MonsterBaseState currentState;

    public MonsterBaseState IdleState { get; protected set; }
    public MonsterBaseState ChaseState { get; protected set; }
    public MonsterBaseState AttackState { get; protected set; }
    
    private void Reset()
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
}
