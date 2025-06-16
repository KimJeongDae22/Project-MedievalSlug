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

    private BossTurretBaseState currentState;
    private BossTurretBaseState prevPattern;

    public List<BossTurretBaseState> Patterns;
    public BossAppearState AppearState { get; private set; }
    public BossAimingState AimingState { get; private set; }

    private int deathCount = 3;

    private void Reset()
    {
        LeftTurret = transform.Find("LeftTurret").GetComponent<BossTurret>();
        RightTurret = transform.Find("RightTurret").GetComponent<BossTurret>();
    }

    private void Awake()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        LeftTurret.Init(BossData,target);
        RightTurret.Init(BossData,target);
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        Patterns = new List<BossTurretBaseState>()
        {
            new Pattern1(this),
            new Pattern2(this),
            new Pattern3(this),
        };

        AppearState = new BossAppearState(this);
        AimingState = new BossAimingState(this);
        //이후 연출을 위해 AppearState로 전환
        ChangeState(AimingState);
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
    
    /// <summary>
    /// 테스트 코드
    /// </summary>
    /// <param name="index"></param>
    public void ChangeState(int index)
    {
        if (index >= Patterns.Count) return;
        currentState?.Exit();
        currentState = Patterns[index];
        currentState?.Enter();
    }
}