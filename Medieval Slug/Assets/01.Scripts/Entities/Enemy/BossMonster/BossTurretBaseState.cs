using UnityEngine;

public abstract class BossTurretBaseState
{
    protected BossTurretStateMachine stateMachine { get; }

    protected BossTurretBaseState(BossTurretStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();
    
    protected void StartAnimation(BossTurret turret, int animatorHash)
    {
        turret.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(BossTurret turret, int animatorHash)
    {
        turret.Animator.SetBool(animatorHash, false);
    }
    
    protected void SetAnimationTrigger(BossTurret turret, int triggerHash)
    {
        turret.Animator.speed = 1f;
        turret.Animator.SetTrigger(triggerHash);
    }
}