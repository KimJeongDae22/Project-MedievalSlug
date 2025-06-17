public class Pattern3 : BasePattern
{
    public Pattern3(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.AllTurretAimTarget(stateMachine.CenterTarget, stateMachine.CenterTarget);
    }
    
    public override void Update()
    {
        
    }
}