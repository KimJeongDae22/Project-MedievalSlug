using System;
using UnityEngine;

public class BossTurret : MonoBehaviour, IDamagable
{
    public BossSO BossData;
    private int health;
    [field : SerializeField] public MonsterAnimationHash AnimationHash { get; private set; }
    [field : SerializeField] public TurretAimingHandler AimingHandler { get; private set;}
    [field : SerializeField] public Animator Animator { get; private set; }
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Vector2 dir;
    public bool IsHalfHealth = false;
    public bool IsDead = false;
    
    protected void Reset()
    {
        AnimationHash = new MonsterAnimationHash();
        AimingHandler = GetComponent<TurretAimingHandler>();
        Animator = GetComponent<Animator>();
        shootPoint = transform.Find("RotatingPivot/ShootPoint").transform;
    }

    public void Init()
    {
        AnimationHash.Initialize();
        health = BossData.Health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            //피격
            if (!IsHalfHealth && health <= BossData.Health / 2f)
            {
                IsHalfHealth = true;
            }
        }
    }

    public void Die()
    {
        IsDead = true;
    }

    public void Fire()
    {
        dir = ((Vector2)shootPoint.position - (Vector2)transform.position).normalized;
        ProjectileManager.Instance.Shoot(dir, shootPoint, ProjectileType.Slime);
    }

    public void ApplyEffect(EffectType effectType)
    {

    }
}