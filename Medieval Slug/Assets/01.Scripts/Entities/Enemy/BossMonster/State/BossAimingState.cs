using UnityEngine;

public class BossAimingState : BossTurretBaseState
{
    public BossAimingState(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    private float timer;
    private bool isFirstPattern = true;

    public override void Enter()
    {
        timer = 0f;
        if (isFirstPattern)
        {
            isFirstPattern = false;
            timer = stateMachine.BossData.PatternCooldown - 5f;
        }
        
        if (!stateMachine.LeftTurret.IsDead) 
            StartAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.IdleParameterHash);
        
        if (!stateMachine.RightTurret.IsDead) 
            StartAnimation(stateMachine.LeftTurret, stateMachine.RightTurret.AnimationHash.IdleParameterHash);
        
        
        stateMachine.AllTurretAim();
    }
    
    public override void Update()
    {
        if (timer <= stateMachine.BossData.PatternCooldown)
        {
            timer += Time.deltaTime;
        }
        else
        {
            stateMachine.ChangeRandomPattern();
        }
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.IdleParameterHash);
        StopAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.IdleParameterHash);
    }
}