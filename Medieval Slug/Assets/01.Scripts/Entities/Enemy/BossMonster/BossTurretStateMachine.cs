using UnityEngine;

public class BossTurretStateMachine : MonoBehaviour
{
    public Transform player;
    public BossTurret leftTurret;
    public BossTurret centerTurret;
    public BossTurret rightTurret;

    private BossTurretBaseState currentState;
    
    

    private void Start()
    {
        
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(BossTurretBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter(this);
    }
}