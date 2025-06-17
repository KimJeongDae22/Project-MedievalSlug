public class BossAppearState : BossTurretBaseState
{
    public BossAppearState(BossTurretStateMachine stateMachine) : base(stateMachine)
    {
    }

    private bool isFirstAnimationFinished = false;
    private bool isFirstAnimationStarted = false;
    private bool isSecondAnimationFinished = false;
    private bool isSecondAnimationStarted = false;
    
    public override void Enter() { }
    
    public override void Update()
    {
        if (!isFirstAnimationStarted)
        {
            StartAnimation(stateMachine.LeftTurret, stateMachine.LeftTurret.AnimationHash.AppearingParameterHash);
            isFirstAnimationStarted = true;
        }
        if (isFirstAnimationStarted && stateMachine.LeftTurret.IsAppeared)
        {
            isFirstAnimationFinished = true;
        }
        
        if (isFirstAnimationFinished && !isSecondAnimationStarted)
        {
            StartAnimation(stateMachine.RightTurret, stateMachine.RightTurret.AnimationHash.AppearingParameterHash);
            isSecondAnimationStarted = true;
        }
        if (isSecondAnimationStarted && stateMachine.RightTurret.IsAppeared)
        {
            isSecondAnimationFinished = true;
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