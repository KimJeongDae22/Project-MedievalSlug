using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private Faction faction;
    [SerializeField] private EffectType curEffectType;

    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private float curduration;

    private Action<GameObject> returnToPool;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 shootDircetion)
    {
        this.direction = shootDircetion.normalized;
        this.curduration = 0f;

        rigidbody.velocity = direction * projectileData.ShotSpeed;
    }

    void Update()
    {
        curduration += Time.deltaTime;
        
        if (curduration >= projectileData.Range)
        {
            OnDespawn();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<IDamagable>() is IDamagable target)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");

            if ((faction == Faction.Player && collision.gameObject.layer == enemyLayer) ||
                (faction == Faction.Enemy && collision.gameObject.layer == playerLayer))
            {
                target.TakeDamage((int)projectileData.Damage);
                target.ApplyEffect(curEffectType);
                OnDespawn();
            }
        }
        else
        {
            OnDespawn();
        }
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }

}
