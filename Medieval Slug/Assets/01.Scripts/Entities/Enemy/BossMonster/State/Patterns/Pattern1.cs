using UnityEngine;

public class Pattern1 : BossTurretBaseState
{
    public Pattern1(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private BossTurret lastTurret;
    private float timer;
    private int step;
    
    public override void Enter()
    {
        stateMachine.AllTurretUnAim();
        
        if (!stateMachine.LeftTurret.IsDead) lastTurret = stateMachine.LeftTurret;
        else if (!stateMachine.RightTurret.IsDead) lastTurret = stateMachine.RightTurret;
        
        timer = 0f;
        step = 0;
    }
    
    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stateMachine.BossData.SequentialFireDelay && step < 3)
        {
            switch (step)
            {
                case 0:
                    if (stateMachine.LeftTurret.IsDead) break;
                    SetAnimationTrigger(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AttackParameterHash);
                    break;
                case 1:
                    if (stateMachine.RightTurret.IsDead) break;
                    SetAnimationTrigger(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AttackParameterHash);
                    break;
                default:
                    break;
            }
            step++;
            timer = 0f;
        }

        if (step == 3 && IsAnimationFinished(lastTurret, "Attack"))
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }
    }

    public override void Exit()
    {
    }
}