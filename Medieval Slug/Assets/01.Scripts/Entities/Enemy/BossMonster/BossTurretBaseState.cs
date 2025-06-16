public abstract class BossTurretBaseState
{
    protected BossTurretStateMachine stateMachine;

    public virtual void Enter(BossTurretStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Exit() { }

    public abstract void Update();
}