using UnityEngine;

public class BossIdleState : BossTurretBaseState
{
    public BossIdleState(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }
    
    float timer = 0f;

    public override void Enter()
    {
        timer = 0f;
        
        if (!stateMachine.LeftTurret.IsDead) 
            StartAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.IdleParameterHash);
        
        //if (!stateMachine.CenterTurret.IsDead) StartAnimation(stateMachine.CenterTurret, stateMachine.CenterTurret.AnimationHash.IdleParameterHash);
        
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
        //StopAnimation(stateMachine.CenterTurret, stateMachine.CenterTurret.AnimationHash.IdleParameterHash);
        StopAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.IdleParameterHash);
    }
}