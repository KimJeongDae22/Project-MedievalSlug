using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossTurretStateMachine : MonoBehaviour
{
    public BossSO BossData;
    public Transform TargetPlayer { get; private set; }
    public Transform LeftMovingTarget { get; private set; }
    public Transform RightMovingTarget { get; private set; }
    public Transform CenterTarget { get; private set; }
    
    public BossTurret LeftTurret;
    public BossTurret RightTurret;
    
    public MovingTarget MovingTargetA { get; private set; }
    public MovingTarget MovingTargetB { get; private set; }
    
    private BossTurretBaseState currentState;
    private BossTurretBaseState prevPattern;

    private List<BossTurretBaseState> Patterns;
    public BossAimingState AimingState { get; private set; }
    public BossAppearState AppearState { get; private set; }
    public BossDefeatState DefeatState { get; private set; }

    private int defeatCount = 0;

    private void Reset()
    {
        LeftTurret = transform.Find("Left/LeftTurret").GetComponent<BossTurret>();
        RightTurret = transform.Find("Right/RightTurret").GetComponent<BossTurret>();
    }

    private void Awake()
    {
        TargetPlayer = GameObject.FindWithTag("Player")?.transform;
        LeftMovingTarget = transform.Find("LeftMovingTarget");
        RightMovingTarget = transform.Find("RightMovingTarget");
        CenterTarget = transform.Find("CenterTarget");

        MovingTargetA = transform.Find("RandomDropPointA").GetComponent<MovingTarget>();
        MovingTargetB = transform.Find("RandomDropPointB").GetComponent<MovingTarget>();
        
        LeftTurret.Init(BossData,TargetPlayer);
        RightTurret.Init(BossData,TargetPlayer);
    }

    private void Start()
    {
        TargetPlayer = GameObject.FindWithTag("Player")?.transform;
        Patterns = new List<BossTurretBaseState>()
        {
            new Pattern1(this),
            new Pattern2(this),
            new Pattern3(this),
        };
        AimingState = new BossAimingState(this);
        AppearState = new BossAppearState(this);
        DefeatState = new BossDefeatState(this);
        
        ChangeState(AppearState);
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(BossTurretBaseState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void AllTurretAim()
    {
        LeftTurret.AimingHandler.IsAniming = true;
        RightTurret.AimingHandler.IsAniming = true;
    }

    public void AllTurretUnAim()
    {
        LeftTurret.AimingHandler.IsAniming = false;
        RightTurret.AimingHandler.IsAniming = false;
    }

    public void AllTurretAimTarget(Transform leftTurretTarget, Transform rightTurretTarget)
    {
        LeftTurret.AimingHandler.SetTarget(leftTurretTarget);
        RightTurret.AimingHandler.SetTarget(rightTurretTarget);
    }

    public void ChangeRandomPattern()
    {
        BossTurretBaseState newPattern = Patterns[Random.Range(0, Patterns.Count)];
        for (int i = 0; i < 1; i++)
        {
            if (newPattern == prevPattern)
            {
                newPattern = Patterns[Random.Range(0, Patterns.Count)];
            }
            else
            {
                break;
            }
        }
        prevPattern = newPattern;
        ChangeState(newPattern);
    }

    public void AddDefeatCount()
    {
        defeatCount++;
        if (defeatCount == 2)
        {
            Defeat();
        }
    }

    public void Defeat()
    {
        StopAllCoroutines();
        LeftTurret.Animator.SetBool(LeftTurret.AnimationHash.DefeatParameterHash, true);
        RightTurret.Animator.SetBool(RightTurret.AnimationHash.DefeatParameterHash, true);
    }
}