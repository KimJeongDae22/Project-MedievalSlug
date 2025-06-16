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
    
    /// <summary>
    /// tag가 붙은 애니메이션이 끝났는지 확인하는 불값 반환 함수
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    protected bool IsAnimationFinished(BossTurret turret, string tag)
    {
        AnimatorStateInfo stateInfo = turret.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag(tag))
        { 
            return stateInfo.normalizedTime >= 1f;
        }
        return false;
    }
}