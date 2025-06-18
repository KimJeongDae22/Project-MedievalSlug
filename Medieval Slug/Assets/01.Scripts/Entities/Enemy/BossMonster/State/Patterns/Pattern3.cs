using System.Collections;
using UnityEngine;

public class Pattern3 : BasePattern
{
    public Pattern3(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }

    private bool isFireing = false;

    public override void Enter()
    {
        base.Enter();
        isFireing = false;
        stateMachine.AllTurretAimTarget(stateMachine.CenterTarget, stateMachine.CenterTarget);
    }
    
    public override void Update()
    {
        bool leftReady = stateMachine.LeftTurret.IsDead || stateMachine.LeftTurret.AimingHandler.IsAimingComplete();
        bool rightReady = stateMachine.RightTurret.IsDead || stateMachine.RightTurret.AimingHandler.IsAimingComplete();
        if (!isFireing && leftReady && rightReady)
        {
            if(!stateMachine.LeftTurret.IsDead) 
                SetAnimationTrigger(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.RandomDropTriggerParameterHash);
            if(!stateMachine.RightTurret.IsDead)
                SetAnimationTrigger(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.RandomDropTriggerParameterHash);
            isFireing = true;
            stateMachine.StartCoroutine(DropBulletsRoutine());
        }
    }
    
    private IEnumerator DropBulletsRoutine()
    {
        yield return new WaitForSeconds(4f);
        int dropCount = 0;
        while (dropCount < 8)
        {
            if(!stateMachine.LeftTurret.IsDead) 
                stateMachine.MovingTargetA.DropBullet();
            if(!stateMachine.RightTurret.IsDead) 
                stateMachine.MovingTargetB.DropBullet();
            dropCount++;
            yield return new WaitForSeconds(0.5f);
        }
        stateMachine.ChangeState(stateMachine.AimingState);
    }
}