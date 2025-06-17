using UnityEngine;

public class Pattern1 : BasePattern
{
    public Pattern1(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private BossTurret lastTurret;
    private float timer;
    private int step;
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.AllTurretUnAim();

        if (!stateMachine.LeftTurret.IsDead)
        {
            lastTurret = stateMachine.LeftTurret;
        }
        if (!stateMachine.RightTurret.IsDead)
        {
            lastTurret = stateMachine.RightTurret;
        }
        
        timer = 0f;
        step = 0;
    }
    
    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stateMachine.BossData.SequentialFireDelay && step < 2)
        {
            switch (step)
            {
                case 0:
                    if (stateMachine.LeftTurret.IsDead) break;
                    SetAnimationTrigger(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AttackTriggerParameterHash);
                    break;
                case 1:
                    if (stateMachine.RightTurret.IsDead) break;
                    SetAnimationTrigger(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AttackTriggerParameterHash);
                    break;
                default:
                    break;
            }
            step++;
            timer = 0f;
        }

        if (step == 2 && timer >= stateMachine.BossData.SequentialFireDelay)
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }
    }
}