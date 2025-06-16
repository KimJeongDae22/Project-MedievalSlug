using System;
using UnityEngine;

public class BossTurret : MonoBehaviour, IDamagable
{
    public BossSO BossData { get; private set; }
    public int Health {get; private set;}
    [field : SerializeField] public MonsterAnimationHash AnimationHash { get; private set; }
    [field : SerializeField] public TurretAimingHandler AimingHandler { get; private set;}
    [field : SerializeField] public Animator Animator { get; private set; }
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Vector2 dir;
    public bool IsHalfHealth = false;
    public bool IsDead = false;
    
    protected void Reset()
    {
        AnimationHash = new MonsterAnimationHash();
        AimingHandler = GetComponent<TurretAimingHandler>();
        Animator = GetComponent<Animator>();
        shootPoint = transform.Find("RotatingPivot/ShootPoint").transform;
        shootingPoint = transform.Find("RotatingPivot/ShootingPoint").transform;
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
                Animator.SetBool("HalfHealth", true);
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
    }

    public void Fire()
    {
        dir = ((Vector2)shootingPoint.position - (Vector2)shootPoint.position).normalized;
        ProjectileManager.Instance.Shoot(dir, shootPoint, ProjectileType.Slime);
    }
}