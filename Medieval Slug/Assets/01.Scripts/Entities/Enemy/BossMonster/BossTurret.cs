using System;
using UnityEngine;

public class BossTurret : MonoBehaviour, IDamagable
{
    [SerializeField] private BossTurretStateMachine stateMachine;
    public BossSO BossData { get; private set; }
    public int Health {get; private set;}
    [field : SerializeField] public MonsterAnimationHash AnimationHash { get; private set; }
    [field : SerializeField] public TurretAimingHandler AimingHandler { get; private set;}
    [field : SerializeField] public Animator Animator { get; private set; }
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Vector2 dir;
    public bool IsHalfHealth { get; private set; } = false;
    public bool IsDead {get; private set;} = false;
    public bool IsAppeared {get; private set;} = false;
    
    protected void Reset()
    {
        stateMachine = transform.parent.parent.GetComponent<BossTurretStateMachine>();
        AnimationHash = new MonsterAnimationHash();
        AimingHandler = GetComponent<TurretAimingHandler>();
        Animator = GetComponent<Animator>();
        shootPoint = transform.Find("RotateForAnim/RotatingPivot/ShootPoint").transform;
        shootingPoint = transform.Find("RotateForAnim/RotatingPivot/ShootingPoint").transform;
    }

    public void Init(BossSO bossData , Transform target)
    {
        BossData = bossData;
        AnimationHash.Initialize();
        Health = BossData.Health;
        AimingHandler.SetTarget(target);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            Animator.SetTrigger(AnimationHash.HitParameterHash);
            if (!IsHalfHealth && Health <= BossData.Health / 2)
            {
                IsHalfHealth = true;
                Animator.speed = 2;
                Animator.SetBool(AnimationHash.HalfHealthParameterHash, true);
            }
        }
    }

    public void ApplyEffect(EffectType effectType)
    {
        
    }

    public void Die()
    {
        IsDead = true;
        Animator.SetTrigger(AnimationHash.DeadParameterHash);
        stateMachine.AddDefeatCount();
    }

    public void Fire()
    {
        dir = ((Vector2)shootingPoint.position - (Vector2)shootPoint.position).normalized;
        ProjectileManager.Instance.Shoot(dir, shootPoint, ProjectileType.Slime);
    }

    public void ApeearingFinished()
    {
        IsAppeared = true;
    }
}