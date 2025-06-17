using UnityEngine;

public class Pattern2 : BasePattern
{
    public Pattern2(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private float timer;
    private int fireCount;
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.AllTurretAimTarget(stateMachine.LeftMovingTarget, stateMachine.RightMovingTarget);
        
        timer = 0f;
        fireCount = 0;
    }
    
    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stateMachine.BossData.SequentialFireDelay && fireCount < 2)
        {
            if (!stateMachine.LeftTurret.IsDead)
            {
                SetAnimationTrigger(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AttackTriggerParameterHash);
            }
            
            if (!stateMachine.RightTurret.IsDead)
            {
                SetAnimationTrigger(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AttackTriggerParameterHash);
            }

            fireCount++;
            timer = 0f;
        }

        if (fireCount == 2 && timer >= stateMachine.BossData.SequentialFireDelay)
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }
    }
}