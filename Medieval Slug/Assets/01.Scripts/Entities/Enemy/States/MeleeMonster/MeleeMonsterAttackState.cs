using UnityEngine;

public class MeleeMonsterAttackState : MonsterBaseState
{
    public MeleeMonsterAttackState(MeleeMonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void EnterState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StartAnimation(stateMachine.Monster.AnimationHash.AttackParameterHash);
    }

    public override void UpdateState()
    {
        if (IsAnimationFinished())
            stateMachine.ChangeState(stateMachine.chaseState);
    }

    public override void ExitState()
    {
        if (stateMachine.Monster.HasAnimator) 
            StopAnimation(stateMachine.Monster.AnimationHash.AttackParameterHash);
    }
}