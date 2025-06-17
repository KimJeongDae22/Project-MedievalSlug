using System.Collections;
using UnityEngine;

public class Pattern3 : BasePattern
{
    public Pattern3(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }

    private bool isFireing = false;
    private int dropCount = 0;

    public override void Enter()
    {
        base.Enter();
        stateMachine.AllTurretAimTarget(stateMachine.CenterTarget, stateMachine.CenterTarget);
    }
    
    public override void Update()
    {
        if (!isFireing && 
            stateMachine.LeftTurret.AimingHandler.IsAimingComplete() &&
            stateMachine.RightTurret.AimingHandler.IsAimingComplete())
        {
            SetAnimationTrigger(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.RandomDropTriggerParameterHash);
            SetAnimationTrigger(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.RandomDropTriggerParameterHash);
            isFireing = true;
            stateMachine.StartCoroutine(DropBulletsRoutine());
        }
    }
    
    private IEnumerator DropBulletsRoutine()
    {
        yield return new WaitForSeconds(4f);
        int dropCount = 0;
        while (dropCount < 4)
        {
            stateMachine.MovingTargetA.DropBullet();
            stateMachine.MovingTargetB.DropBullet();
            dropCount++;
            yield return new WaitForSeconds(1f);
        }
        stateMachine.ChangeState(stateMachine.AimingState);
    }
}