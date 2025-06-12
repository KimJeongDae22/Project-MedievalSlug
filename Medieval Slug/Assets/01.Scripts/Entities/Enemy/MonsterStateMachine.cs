using UnityEngine;

public abstract class MonsterStateMachine : MonoBehaviour
{
    [field : SerializeField] public Monster Monster { get; private set; }
    [field : SerializeField] public Transform target { get; private set; }
    [field : SerializeField] public LayerMask targetLayer { get; private set; }
    
    protected MonsterBaseState currentState;

    public MonsterBaseState idleState { get; protected set; }
    public MonsterBaseState chaseState { get; protected set; }
    public MonsterBaseState attackState { get; protected set; }
    
    private void Reset()
    {
        Monster = GetComponent<Monster>();
        target = GameObject.FindWithTag("Player")?.transform;
        targetLayer = LayerMask.GetMask("Player");
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
