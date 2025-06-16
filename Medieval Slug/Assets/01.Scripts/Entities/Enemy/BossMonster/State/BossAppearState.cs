public class BossAppearState : BossTurretBaseState
{
    public BossAppearState(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }

    private bool isFirstAnimationFinished = false;
    private bool isSecondAnimationFinished = false;

    public override void Enter() { }
    
    public override void Update()
    {
        if (!isFirstAnimationFinished)
        {
            StartAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AppearingParameterHash);
            if (IsAnimationFinished(stateMachine.LeftTurret, "Appearing"))
            {
                isFirstAnimationFinished = true;
            }
        }

        if (isFirstAnimationFinished && !isSecondAnimationFinished)
        {
            StartAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AppearingParameterHash);
            if (IsAnimationFinished(stateMachine.RightTurret, "Appearing"))
            {
                isSecondAnimationFinished = true;
            }
        }

        if (isFirstAnimationFinished && isSecondAnimationFinished)
        {
            stateMachine.ChangeState(stateMachine.AimingState);
        }
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AppearingParameterHash);
        StopAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AppearingParameterHash);
    }
}