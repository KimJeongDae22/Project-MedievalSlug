using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossTurretStateMachine : MonoBehaviour
{
    public BossSO BossData;
    public Transform target;
    public BossTurret LeftTurret;
    public BossTurret RightTurret;
    public BossTurret CenterTurret;

    private BossTurretBaseState currentState;
    private BossTurretBaseState prevPattern;

    public List<BossTurretBaseState> Patterns;
    public BossAppearState AppearState { get; private set; }
    public BossIdleState IdleState { get; private set; }

    private int deathCount = 3;

    private void Reset()
    {
        LeftTurret = transform.Find("LeftTurret").GetComponent<BossTurret>();
        //CenterTurret = transform.Find("CenterTurret").GetComponent<BossTurret>();
        RightTurret = transform.Find("RightTurret").GetComponent<BossTurret>();
    }

    private void Awake()
    {
        LeftTurret.BossData = BossData;
        RightTurret.BossData = BossData;
        //CenterTurret.BossData = BossData;
        LeftTurret.Init();
        RightTurret.Init();
        //CenterTurret.Init();
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        Patterns = new List<BossTurretBaseState>()
        {
            new Pattern1(this),
            //new Pattern2(this),
            //new Pattern3(this),
        };

        AppearState = new BossAppearState(this);
        IdleState = new BossIdleState(this);
        //이후 연출을 위해 AppearState로 전환
        ChangeState(IdleState);
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
        LeftTurret.AimingHandler.isAniming = true;
        RightTurret.AimingHandler.isAniming = true;
        //CenterTurret.AimingHandler.isAniming = true;
    }

    public void AllTurretUnAim()
    {
        LeftTurret.AimingHandler.isAniming = false;
        RightTurret.AimingHandler.isAniming = false;
        //CenterTurret.AimingHandler.isAniming = false;
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
}