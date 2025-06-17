using UnityEngine;

public abstract class BasePattern : BossTurretBaseState
{
    protected BasePattern(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private float timer;
    private int step;
    
    public override void Enter()
    {
        if (!stateMachine.LeftTurret.IsDead)
        {
            StartAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AttackParameterHash);
        }
        if (!stateMachine.RightTurret.IsDead)
        {
            StartAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AttackParameterHash);
        }
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AttackParameterHash);
        StopAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AttackParameterHash);
        stateMachine.AllTurretAimTarget(stateMachine.TargetPlayer, stateMachine.TargetPlayer);
    }
}